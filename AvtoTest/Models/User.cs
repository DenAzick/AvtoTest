﻿using Autotest.Mvc.Services;
using Avtotest.Models;
using AvtoTest.Repositories;

namespace Autotest.Mvc.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? PhotoPath { get; set; }
    public int? CurrentTicketIndex { get; set; }
    public Ticket? CurrentTicket { get; set; }

    public Ticket? CorrectCountIndex { get; set; }
    public int CorrectCount { get; set; }
    public List<Ticket> Tickets { get; set; }
}