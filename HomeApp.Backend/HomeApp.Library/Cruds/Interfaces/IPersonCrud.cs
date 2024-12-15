// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;

namespace HomeApp.Library.Cruds;

public interface IPersonCrud
{
    Task CreateAsync(Person person, CancellationToken cancellationToken);
    Task DeleteAsync(int userId, CancellationToken cancellationToken);
    Task<PersonDto> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Person person, CancellationToken cancellationToken);
}
