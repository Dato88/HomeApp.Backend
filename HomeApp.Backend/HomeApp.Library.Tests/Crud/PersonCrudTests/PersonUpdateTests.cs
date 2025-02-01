namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class PersonUpdateTests : BasePersonTest
{
    // public PersonUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    // [Fact]
    // public async Task UpdateAsync_ShouldUpdatePerson_WhenPersonExists()
    // {
    //     // Arrange
    //     Person existingPerson = new()
    //     {
    //         Username = "testuser",
    //         FirstName = "John",
    //         LastName = "Doe",
    //         Email = "test@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     DbContext.People.Add(existingPerson);
    //     await DbContext.SaveChangesAsync();
    //
    //     Person updatedPerson = new()
    //     {
    //         Id = existingPerson.Id,
    //         Username = "updateduser",
    //         FirstName = "Jane",
    //         LastName = "Doe",
    //         Email = "updated@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
    //     };
    //
    //     // Act
    //     await _personCrud.UpdateAsync(updatedPerson, default);
    //
    //     // Assert
    //     existingPerson.Id.Should().Be(1);
    //     existingPerson.Username.Should().Be(updatedPerson.Username);
    //     existingPerson.FirstName.Should().Be(updatedPerson.FirstName);
    //     existingPerson.LastName.Should().Be(updatedPerson.LastName);
    //     existingPerson.Email.Should().Be(updatedPerson.Email);
    //     existingPerson.UserId.Should().Be(updatedPerson.UserId);
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_ShouldCall_AllValidations()
    // {
    //     // Arrange
    //     Person existingPerson = new()
    //     {
    //         Username = "testuser",
    //         FirstName = "John",
    //         LastName = "Doe",
    //         Email = "test@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     DbContext.People.Add(existingPerson);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     Person updatedPerson = new()
    //     {
    //         Id = existingPerson.Id,
    //         Username = "updateduser",
    //         FirstName = "Jane",
    //         LastName = "Doe",
    //         Email = "updated@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
    //     };
    //
    //     await _personCrud.UpdateAsync(updatedPerson, default);
    //
    //     // Assert
    //     var result = await DbContext.People.FindAsync(existingPerson.Id);
    //
    //     _personValidationMock.Verify(
    //         v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
    //         Times.Once);
    //     _personValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
    //     _personValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
    //     _personValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_ShouldNotCall_ValidatePersonnameDoesNotExistAsync()
    // {
    //     // Arrange
    //     Person existingPerson = new()
    //     {
    //         Username = "testuser",
    //         FirstName = "John",
    //         LastName = "Doe",
    //         Email = "test@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     DbContext.People.Add(existingPerson);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     Person updatedPerson = new()
    //     {
    //         Id = existingPerson.Id,
    //         Username = "testuser",
    //         FirstName = "Jane",
    //         LastName = "Doe",
    //         Email = "updated@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
    //     };
    //
    //     await _personCrud.UpdateAsync(updatedPerson, default);
    //
    //     // Assert
    //     var result = await DbContext.People.FindAsync(existingPerson.Id);
    //
    //     _personValidationMock.Verify(
    //         v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
    //         Times.Never);
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_ShouldThrowException_WhenPersonIsNull()
    // {
    //     // Act
    //     var action = async () => await _personCrud.UpdateAsync(null, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<ArgumentNullException>();
    // }

    // [Fact]
    // public async Task UpdateAsync_ShouldThrowException_WhenPersonNotFound()
    // {
    //     // Arrange
    //     Person nonExistingPerson = new()
    //     {
    //         Id = 999,
    //         Username = "nonexistinguser",
    //         FirstName = "John",
    //         LastName = "Doe",
    //         Email = "nonexisting@example.com",
    //         UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
    //     };
    //
    //     // Act
    //     var action = async () => await _personCrud.UpdateAsync(nonExistingPerson, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(PersonMessage.PersonNotFound);
    // }
}
