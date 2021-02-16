using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1Tests.Shared;
using WebApplication1.Models;
using WebApplication1.Models.KlientRequest;
using WebApplication1.Models.ServerResultat;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class BetalingsPlanControllerTests : SharedDataBase
    {

        [TestMethod()]
        public void PostBetalingsPlanTest()
        {
            using var context = new BankContext(true, ContextOptions);
            var controller = new BetalingsPlanController(context);

            var res = controller.PostBetalingsPlan(new BetalingsPlanInfo
            {
                Aar = 20,
                LaaneTypeId = 1,
                SkjemaId = 0,
                Sum = 1_000_000,
            });

            Assert.IsInstanceOfType(res.Result, typeof(OkObjectResult));

            BetalingsPlan plan = (BetalingsPlan)((ObjectResult)res.Result).Value;
            Assert.IsTrue(plan.Betalinger.Length == 20 * 12);


            var tom = controller.PostBetalingsPlan(null);
            Assert.IsInstanceOfType(tom.Result, typeof(BadRequestResult));

            var ugyldigLåneType = controller.PostBetalingsPlan(new BetalingsPlanInfo
            {
                Aar = 20,
                LaaneTypeId = 100,
                SkjemaId = 1,
                Sum = 10000000
            });

            Assert.IsInstanceOfType(ugyldigLåneType.Result, typeof(NotFoundObjectResult));

            var ugyldigSkjemaType = controller.PostBetalingsPlan(new BetalingsPlanInfo
            {
                Aar = 20,
                LaaneTypeId = 1,
                SkjemaId = 100,
                Sum = 1000000
            });
            Assert.IsInstanceOfType(ugyldigSkjemaType.Result, typeof(NotFoundObjectResult));
        }
    }
}