﻿using Bottles.Deployment;
using Bottles.Deployment.Deployers;
using Bottles.Exploding;
using Bottles.Zipping;
using FubuCore;
using NUnit.Framework;

namespace Bottles.Tests.Deployment
{
    [TestFixture]
    public class IntegrationIisFubuDeploymentTester
    {
        [Test][Explicit]
        public void DeployHelloWorld()
        {
            IFileSystem fileSystem = new FileSystem();
            IProfile profile = new Profile(@"C:\dev\test-profile\");
            IBottleRepository bottles = new BottleRepository(fileSystem, profile, new PackageExploder(new ZipFileService(fileSystem), new PackageExploderLogger(s=>{ }), fileSystem ));



            var deployer = new IisFubuDeployer(fileSystem, bottles);

            var directive = new IisFubuWebsite();
            directive.HostBottle = "test";
            directive.WebsiteName = "fubu";
            directive.WebsitePhysicalPath = @"C:\dev\test-web";
            directive.VDir = "bob";
            directive.VDirPhysicalPath = @"C:\dev\test-app";
            directive.AppPool = "fubizzle";

            directive.DirectoryBrowsing = Activation.Enable;


            deployer.Deploy(directive);
        }
        
    }
}