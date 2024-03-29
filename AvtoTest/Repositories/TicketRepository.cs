﻿namespace AvtoTest.Repositories;
using Autotest.Mvc.Models;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Net.Sockets;

public class TicketRepository
{
    private readonly SqliteConnection _connection;
    public TicketRepository()
    {
        _connection = new SqliteConnection("Data source=autotest.db");

        _connection.Open();
        CreareTicketTable();
    }

    private void CreareTicketTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS tickets(" +
                            "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                            "ticket_id INTEGER," +
                            "user_id TEXT, " +
                            "questions_count INTEGER, " +
                            "start_index INTEGER, " +
                            "current_question_index INTEGER, " +
                            "date BIGINT)";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS ticket_answers(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ticket_id INTEGER, " +
            "question_index INTEGER, " +
            "choice_index INTEGER, " +
            "correct_index INTEGER)";
        command.ExecuteNonQuery();

    }

    public void AddTicket(Ticket ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO tickets(user_id, " +
            "ticket_id, " +
            "questions_count, " +
            "start_index, " +
            "current_question_index, " +
            "date)" +
            " VALUES (@user_id, " +
            "@ticket_id, " +
            "@questions_count, " +
            "@start_index, " +
            "@current_question_index, " +
            "@date)";

        command.Parameters.AddWithValue("user_id", ticket.UserId);
        command.Parameters.AddWithValue("ticket_id", ticket.TicketId);
        command.Parameters.AddWithValue("questions_count", ticket.QuestionsCount);
        command.Parameters.AddWithValue("start_index", ticket.StartIndex);
        command.Parameters.AddWithValue("current_question_index", ticket.CurrentQuestionIndex);
        command.Parameters.AddWithValue("date", ticket.Date.Ticks);

        command.Prepare();
        command.ExecuteNonQuery();
    }
    public void DeleteTicketAnswers(int ticketId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM ticket_answers WHERE ticket_id = @id";

        command.Parameters.AddWithValue("id", ticketId);

        command.Prepare();
        command.ExecuteNonQuery();
    }


    public void UpdateTicket(Ticket ticket)
    {
        var command = _connection.CreateCommand();

        command.CommandText = "UPDATE tickets SET current_question_index = @current_question_index, date = @date WHERE id = @id";

        command.Parameters.AddWithValue("id", ticket.TicketId);
        command.Parameters.AddWithValue("current_question_index", ticket.CurrentQuestionIndex);
        command.Parameters.AddWithValue("date", ticket.Date.Ticks);

        command.Prepare();
        command.ExecuteNonQuery();
    }

    public Ticket? GetTicket(int ticketId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM tickets WHERE id = @id";

        command.Parameters.AddWithValue("id", ticketId);

        command.Prepare();
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var ticket = new Ticket()
            {
                TicketId = reader.GetInt32(1),
                UserId = reader.GetString(2),
                QuestionsCount = reader.GetInt32(3),
                StartIndex = reader.GetInt32(4),
                CurrentQuestionIndex = reader.GetInt32(5),
                Date = DateTime.FromFileTime(reader.GetInt64(6)),

            };

            reader.Close();
            return ticket;

        }
        reader.Close();
        return null;
    }

    public List<Ticket> GetTicketList(string userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM tickets WHERE user_id = @id";
        command.Parameters.AddWithValue("id", userId);
        command.Prepare();

        var reader = command.ExecuteReader();

        var tickets = new List<Ticket>();

        while (reader.Read())
        {
            var ticket = new Ticket()
            {
                TicketId = reader.GetInt32(1),
                UserId = reader.GetString(2),
                QuestionsCount = reader.GetInt32(3),
                StartIndex = reader.GetInt32(4),
                CurrentQuestionIndex = reader.GetInt32(5),
                Date = DateTime.FromFileTime(reader.GetInt64(6))

            };

            tickets.Add(ticket);
            
        }
        reader.Close();
        return tickets;
    }

    public void AddTicketAnswer(TicketQuestionAnswer answer)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO ticket_answers(ticket_id ,question_index, choice_index, correct_index)" +
            " VALUES (@ticket_id ,@question_index, @choice_index, @correct_index)";

        command.Parameters.AddWithValue("ticket_id", answer.TicketId);
        command.Parameters.AddWithValue("question_index", answer.QuestionIndex);
        command.Parameters.AddWithValue("choice_index", answer.ChoiceIndex);
        command.Parameters.AddWithValue("correct_index", answer.CorrectIndex);

        command.Prepare();
        command.ExecuteNonQuery();
    }

    public List<TicketQuestionAnswer> GetTicketQuestionAnswers(int ticketId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM ticket_answers WHERE ticket_id = @id";

        command.Parameters.AddWithValue("id", ticketId);

        command.Prepare();
        var reader = command.ExecuteReader();

        var ticket_answers = new List<TicketQuestionAnswer>();

        while (reader.Read())
        {
           var answer =  new TicketQuestionAnswer()
            {
                Id = reader.GetInt32(0),
                TicketId = reader.GetInt32(1),
                QuestionIndex = reader.GetInt32(3),
                ChoiceIndex = reader.GetInt32(4),
                CorrectIndex = reader.GetInt32(5),
            };

            ticket_answers.Add(answer);
            
        }
        reader.Close();
        return ticket_answers;
    }

}



