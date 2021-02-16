using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1Tests.Shared;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Tests
{
    [TestClass()]
    public class KunderControllerTests : SharedDataBase
    {

        [TestMethod()]
        public void GetKundeTest()
        {
            using var context = new BankContext(true,ContextOptions);
            using var controller = new KunderController(context);

            var res = controller.GetKunde(1);

            Assert.IsInstanceOfType(res.Result, typeof(OkObjectResult));

            Kunde kunde = (Kunde)((ObjectResult)res.Result).Value;

            Assert.IsTrue(kunde.Id == 1);

            var tom = controller.GetKunde(null);
            Assert.IsInstanceOfType(tom.Result, typeof(NotFoundResult));

            var ukjent = controller.GetKunde(1000);
            Assert.IsInstanceOfType(ukjent.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void RegisterTest()
        {
            using var context = new BankContext(true,ContextOptions);
            using var controller = new KunderController(context);

            var res = controller.Register(new Kunde
            {
                Id = 5,
                Fornavn = "Knut",
                Etternavn = "Hagen"
            });

            Assert.IsInstanceOfType(res.Result, typeof(OkResult));

            var uthent = controller.GetKunde(5);

            Assert.IsInstanceOfType(uthent.Result, typeof(OkObjectResult));

            var kunde = (Kunde)((ObjectResult)uthent.Result).Value;

            Assert.IsTrue(kunde.Id == 5 && kunde.Fornavn.Equals("Knut") && kunde.Etternavn.Equals("Hagen"));

        }

        [TestMethod()]

        public void DeleteTest()
        {

            using var context = new BankContext(true, ContextOptions);
            using var controller = new KunderController(context);
            using var lController = new LaanController(context); //brukt for å hente lånene til kunden vi skal fjerne

            var lånr = lController.GetLånForKunde(1);

            Assert.IsInstanceOfType(lånr.Result, typeof(OkObjectResult));

            var lån = (IEnumerable<Laan>)((ObjectResult)lånr.Result).Value;



            var slett = controller.Delete(1);
            Assert.IsInstanceOfType(slett.Result, typeof(OkResult));

            var hent = controller.GetKunde(1);
            Assert.IsInstanceOfType(hent.Result, typeof(NotFoundResult));

            //Prøver å hente ut assosierte lån for den slettede kunden
            //Det skal resultere i 404 error
            lånr = lController.GetLånForKunde(1);

            Assert.IsInstanceOfType(lånr.Result,typeof(NotFoundResult));
        }
    }
}