# AutoNSubstitute.Shortcuts
 The weight loss therapy for .Net unit tests using AutoFixture and NSubstitute

 ```
    IFixture fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
    fixture.Customizations.Add(new SubstituteRelay(SubstituteSpecification
        .For<HomeController>()
        .With<ControllerContext>()
        .Create()));

    // IMPORTANT: freeze the types to be manipulated before create the cut.
    fixture.Freeze<UserService, UserRepository>();

    // The cut is all set to go.
    var cut = fixture.Create<HomeController>();

    // Automate the substitutes through the fixture
    fixture.ReturnsAuto<UserService, User>(async service => await service.GetAsync(default));
    Assert.That((await cut.GetUserAsync(default)).Number < 100);

    // Automate the errors through the fixture
    fixture.ThrowsAsyncAuto<UserRepository, NetworkInformationException>(async repo => await repo.GetAsync(default));
    Assert.ThrowsAsync<NetworkInformationException>(async () => await cut.GetUserAsync(default));
 ```