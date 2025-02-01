namespace HomeApp.Library.Tests.Facades;

public class PersonFacadeTests : BasePersonFacadeTest
{
    // public PersonFacadeTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task CreatePersonAsync_CreatesPersonSuccessfully()
    // {
    //     // Arrange
    //     var person = new Person
    //     {
    //         Username = "john.doe",
    //         FirstName = "John",
    //         LastName = "Doe",
    //         Email = "john.doe@example.com",
    //         UserId = Guid.NewGuid().ToString()
    //     };
    //
    //     // Act
    //     await _personFacade.CreatePersonAsync(person, default);
    //
    //     // Assert
    //     _personCrudMock.Verify(x => x.CreateAsync(person, It.IsAny<CancellationToken>()), Times.Once);
    // }
    //
    // [Fact]
    // public async Task GetPeopleAsync_ReturnsEmptyList_WhenNoPeopleExist()
    // {
    //     // Arrange
    //     _personCrudMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()))
    //         .ReturnsAsync(new List<PersonDto>());
    //
    //     // Act
    //     var result = await _personFacade.GetPeopleAsync(default);
    //
    //     // Assert
    //     result.Should().BeEmpty();
    // }
    //
    // [Fact]
    // public async Task GetPersonByEmailAsync_ReturnsPerson_WhenPersonExists()
    // {
    //     // Arrange
    //     var email = "john.doe@example.com";
    //     var personDto = new PersonDto(1, email, "John", "Doe", email);
    //
    //     _personCrudMock.Setup(x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()))
    //         .ReturnsAsync(personDto);
    //
    //     // Act
    //     var result = await _personFacade.GetPersonByEmailAsync(email, default);
    //
    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Email.Should().Be(email);
    //     _personCrudMock.Verify(
    //         x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()), Times.Once);
    // }
    //
    // [Fact]
    // public async Task GetPersonByEmailAsync_ReturnsNull_WhenPersonDoesNotExist()
    // {
    //     // Arrange
    //     var email = "nonexistent@example.com";
    //
    //     _personCrudMock.Setup(x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()))
    //         .ReturnsAsync((PersonDto)null);
    //
    //     // Act
    //     var result = await _personFacade.GetPersonByEmailAsync(email, default);
    //
    //     // Assert
    //     result.Should().BeNull();
    //     _personCrudMock.Verify(
    //         x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()), Times.Once);
    // }
    //
    // [Fact]
    // public async Task DeletePersonAsync_RemovesPersonSuccessfully()
    // {
    //     // Arrange
    //     var personId = 1;
    //
    //     // Act
    //     await _personFacade.DeletePersonAsync(personId, default);
    //
    //     // Assert
    //     _personCrudMock.Verify(x => x.DeleteAsync(personId, It.IsAny<CancellationToken>()), Times.Once);
    // }
    //
    // [Fact]
    // public async Task UpdatePersonAsync_UpdatesPersonWithNewDetails()
    // {
    //     // Arrange
    //     var person = new Person
    //     {
    //         Id = 1,
    //         Username = "jane.doe",
    //         FirstName = "Jane",
    //         LastName = "Doe",
    //         Email = "jane.doe@example.com",
    //         UserId = Guid.NewGuid().ToString()
    //     };
    //
    //     // Act
    //     await _personFacade.UpdatePersonAsync(person, default);
    //
    //     // Assert
    //     _personCrudMock.Verify(x => x.UpdateAsync(person, It.IsAny<CancellationToken>()), Times.Once);
    // }
}
