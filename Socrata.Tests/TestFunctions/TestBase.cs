using System;
using System.IO;
using System.Reflection;
using System.Linq;
using NUnit.Framework;

namespace Socrata
{
    public class TestBase {
        [SetUp]
        public void Setup()
        {
            this.DeployItems();
        }

        private void DeployItems()
        {
            var currentType = this.GetType();
            var publicMethodsWithTestAttr = currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CustomAttributes.Any(y=>y.AttributeType == typeof(TestAttribute)));

            // Iterate through each test method and deploy files as necessary
            foreach (MethodInfo testMethodInfo in publicMethodsWithTestAttr)
            {
                var deploymentItemAttrs = testMethodInfo.GetCustomAttributes(typeof(DeploymentItemAttribute));
                foreach (DeploymentItemAttribute deploymentItemAttr in deploymentItemAttrs)
                {
                    this.DeployFile(deploymentItemAttr.Path,  deploymentItemAttr.OutputDirectory);
                }
            }
        }

        private void DeployFile(string path, string outputDirectory = null)
        {
            string filePath = path; // path.Replace("/", "\\");
            
            // _environmentDir = new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo _environmentDir = new DirectoryInfo(Environment.CurrentDirectory);
            string originalItemPath = new Uri(Path.Combine(_environmentDir.Parent.Parent.Parent.FullName, "TestFunctions", filePath)).LocalPath;
            string originalItemName = Path.GetFileName(originalItemPath);

            string runFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("DeploymentItem: Copying " + originalItemPath + " to " + runFolderPath);

            string itemPathInBin;
            if (string.IsNullOrEmpty(outputDirectory))
            {
                itemPathInBin = new Uri(Path.Combine(runFolderPath, originalItemName)).LocalPath;
            }
            else if (!string.IsNullOrEmpty(Path.GetPathRoot(outputDirectory)))
            {
                itemPathInBin = new Uri(Path.Combine(outputDirectory, originalItemName)).LocalPath;
            }
            else
            {
                itemPathInBin = new Uri(Path.Combine(runFolderPath, outputDirectory, originalItemName)).LocalPath;
            }

            if (File.Exists(originalItemPath)) // It's a file
            {
                string parentFolderPathInBin = new DirectoryInfo(itemPathInBin).Parent.FullName;

                if (!Directory.Exists(parentFolderPathInBin))
                {
                    Directory.CreateDirectory(parentFolderPathInBin);
                }

                File.Copy(originalItemPath, itemPathInBin, true);

                FileAttributes fileAttributes = File.GetAttributes(itemPathInBin);
                if ((fileAttributes & FileAttributes.ReadOnly) != 0)
                {
                    File.SetAttributes(itemPathInBin, fileAttributes & ~FileAttributes.ReadOnly);
                }
            }
            else if (Directory.Exists(originalItemPath)) // It's a folder
            {
                if (Directory.Exists(itemPathInBin))
                {
                    Directory.Delete(itemPathInBin, true);
                }

                Directory.CreateDirectory(itemPathInBin);

                foreach (string dirPath in Directory.GetDirectories(originalItemPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(originalItemPath, itemPathInBin));
                }

                foreach (string sourcePath in Directory.GetFiles(originalItemPath, "*.*", SearchOption.AllDirectories))
                {
                    string destinationPath = sourcePath.Replace(originalItemPath, itemPathInBin);
                    File.Copy(sourcePath, destinationPath, true);
                    FileAttributes fileAttributes = File.GetAttributes(destinationPath);
                    if ((fileAttributes & FileAttributes.ReadOnly) != 0)
                    {
                        File.SetAttributes(destinationPath, fileAttributes & ~FileAttributes.ReadOnly);
                    }
                }
            }
            else
            {
                Console.WriteLine("Warning: Deployment item does not exist - \"" + originalItemPath + "\"");
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class DeploymentItemAttribute : Attribute
{
    public DeploymentItemAttribute(string path, string outputDirectory = null)
    {
        this.Path = path;
        this.OutputDirectory = outputDirectory;
    }

    public string Path { get; set; }

    public string OutputDirectory { get; set; }
}
