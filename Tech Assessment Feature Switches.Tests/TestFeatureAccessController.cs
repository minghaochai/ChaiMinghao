using System;
using Moq;
using Xunit;
using Tech_Assessment_Feature_Switches.Controllers;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Tech_Assessment_Feature_Switches.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tech_Assessment_Feature_Switches.test
{
    public class TestFeatureAccessController
    {
        private FeatureAccessController controller;

        [Fact]
        public async Task FeatureAccessController_GetFeatureAccessExistingRecord_ReturnSuccess()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser1",
                FeatureName = "Delete",
                Enable = false
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            // Act
            var result = await controller.GetFeatureAccess("testUser1", "Delete");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task FeatureAccessController_GetFeatureAccessNonExistentRecord_ReturnBadRequest1()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser1",
                FeatureName = "Insert",
                Enable = false
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            // Act
            var result = await controller.GetFeatureAccess("testUser1", "Upload");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task FeatureAccessController_GetFeatureAccessNonExistentRecord_ReturnBadRequest2()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser1",
                FeatureName = "Append",
                Enable = false
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            // Act
            var result = await controller.GetFeatureAccess("testUser5", "Insert");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task FeatureAccessController_UpdateUserFeatureAccessForExistingRecord_ReturnSuccess()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Export",
                Enable = true
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };
            // Act
            FeatureAccess sampleBody = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Export",
                Enable = false
            };

            var result = await controller.UpdateUserFeatureAccess(sampleBody);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task FeatureAccessController_UpdateUserFeatureAccessForExistingRecord_ReturnNotModified1()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Import",
                Enable = true
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            // Act
            FeatureAccess sampleBody = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Import",
                Enable = true
            };

            var result = await controller.UpdateUserFeatureAccess(sampleBody);

            // Assert
            Assert.True((result.Result as StatusCodeResult).StatusCode == 304);
        }

        [Fact]
        public async Task FeatureAccessController_UpdateUserFeatureAccessForNonExistentRecord_ReturnNotModified2()
        {
            var options = new DbContextOptionsBuilder<FeatureAccessContext>()
                            .UseInMemoryDatabase(databaseName: "FeatureAccessList")
                            .Options;

            var record = new FeatureAccess
            {
                Email = "testUser3",
                FeatureName = "Import",
                Enable = true
            };

            FeatureAccessContext _context = new FeatureAccessContext(options);
            _context.FeatureAccess.Add(record);
            _context.SaveChanges();

            controller = new FeatureAccessController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            // Act
            FeatureAccess sampleBody = new FeatureAccess
            {
                Email = "testUser5",
                FeatureName = "Export",
                Enable = true
            };

            var result = await controller.UpdateUserFeatureAccess(sampleBody);

            // Assert
            Assert.True((result.Result as StatusCodeResult).StatusCode == 304);
        }
    }
}