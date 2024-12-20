﻿using HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.Library.Facades.Interfaces;

public interface ITodoFacade
{
    Task<GetToDoDto> CreateTodoAsync(CreateToDoDto createToDoDto, CancellationToken cancellationToken);
    Task<IEnumerable<GetToDoDto>> GetTodosAsync(CancellationToken cancellationToken);
}