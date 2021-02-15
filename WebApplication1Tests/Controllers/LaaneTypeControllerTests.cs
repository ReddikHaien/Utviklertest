﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1Tests.Shared;
using Xunit;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class LaaneTypeControllerTests : SharedDataBase
    {

        public LaaneTypeControllerTests() : base()
        {
        }

        
        [TestMethod()]
        public async Task GetLåneTyper()
        {
            using var context = new BankContext(true,ContextOptions);
            LaaneTypeController controller = new LaaneTypeController(context);

            var type = await controller.GetLåneTyper();

            Assert.IsInstanceOfType(type.Result.GetType(), typeof(OkObjectResult));
        }

        [TestMethod()]
        public async Task GetLåneTypeMedID()
        {
            using var context = new BankContext(true,ContextOptions);
            LaaneTypeController controller = new LaaneTypeController(context);

            //Tester for Ok med en som eksisterer
            var boliglån = await controller.GetLåneType(1);

            
            //Tester for not found med nullid
            var manglerID = await controller.GetLåneType(null);
            Assert.IsInstanceOfType(manglerID.Result, typeof(NotFoundResult));
            //Tester for not found med en ikke eksiterende ID

            var ukjentID = await controller.GetLåneType(10000);
            Assert.IsInstanceOfType(ukjentID.Result, typeof(NotFoundResult));
        }
    }
}