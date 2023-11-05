using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoNSubstitute.Shortcuts.Test.Example;
using NUnit.Framework;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AutoNSubstitute.Shortcuts.Test
{
    [TestFixture]
    public class SubstituteSpecificationTests
    {
        [Datapoint]
        (string, SubstituteSpecification) forType =
        (
            nameof(SubstituteSpecification.For),
            SubstituteSpecification
                .For<HomeController>()
                .With<ControllerContext>()
                .Create()
        );

        [Datapoint]
        (string, SubstituteSpecification) forNamespace = ( 
            nameof(SubstituteSpecification.ForNamespace), 
            SubstituteSpecification
                .ForNamespace(nameof(AutoNSubstitute))
                .Except<HomeController>()
                .With<ControllerContext>()
                .Create());

        [Theory]
        public async Task Test_SubstituteSpecification_ForType((string, SubstituteSpecification) datapoint)
        {
            var (method, specification) = datapoint;
            Assume.That(method == nameof(SubstituteSpecification.For));
            
            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            fixture.Customizations.Add(new SubstituteRelay(specification));

            fixture.Freeze<UserService>();
            var cut = fixture.Create<HomeController>();
            Assert.That(cut.Service.Repository.Users.Count > 0);

            fixture.Returns<UserService, User>(async service => await service.GetAsync(default), new User { Number = 777 });
            Assert.That((await cut.GetUserAsync(default)).Number == 777);

            fixture.ReturnsAuto<UserService, User>(async service => await service.GetAsync(default));
            Assert.That((await cut.GetUserAsync(default)).Number < 100);

            fixture.ThrowsAsyncAuto<UserService, NetworkInformationException>(async service => await service.GetAsync(default));
            Assert.ThrowsAsync<NetworkInformationException>(async () => await cut.GetUserAsync(default));
        }

        [Theory]
        public async Task Test_SubstituteSpecification_ForNamespace((string, SubstituteSpecification) datapoint)
        {
            var (method, specification) = datapoint;
            Assume.That(method == nameof(SubstituteSpecification.ForNamespace));

            IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            fixture.Customizations.Add(new SubstituteRelay(specification));

            fixture.Freeze<UserService>();
            var cut = fixture.Create<HomeController>();
            Assert.That(cut.Service.Repository.Users.Count > 0);

            fixture.ReturnsAuto<UserService, User>(async service => await service.GetAsync(default));
            Assert.That((await cut.GetUserAsync(default)).Number < 100);

            fixture.Customize<User>(composer => composer.With(user => user.Number, 777));
            fixture.ReturnsAuto<UserService, User>(service => service.Get(default));
            Assert.That(cut.GetUser(default).Number == 777);

            fixture.ThrowsAuto<UserService, UnauthorizedAccessException>(service => service.Get(default));
            Assert.Throws<UnauthorizedAccessException>(() => cut.GetUser(default));
        }
    }
}