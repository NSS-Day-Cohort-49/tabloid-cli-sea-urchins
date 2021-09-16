using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;


namespace TabloidCLI.UserInterfaceManagers
{
    class NoteManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private string _connectionString;
        private PostRepository _postRepository;

        public NoteManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _connectionString = connectionString;
            _postRepository = new PostRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Menu");
            Console.WriteLine(" 1) List Notes");
            Console.WriteLine(" 2) Add Note");
            Console.WriteLine(" 3) Remove Note");
            Console.WriteLine(" 0) Return to main menu");

            Console.WriteLine("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "0":
                    return _parentUI;
                case "2":
                    Add();
                    return this;
                case "3":
                    Remove();
                    return this;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Note> notes = _noteRepository.GetAll();
            foreach (Note note in notes)
            {
                Console.WriteLine(note);
            }
        }

        private void Add()
        {
            List<Post> posts = _postRepository.GetAll();
            Post poster = null;

            Console.WriteLine("New ");
            Note note = new Note();

            Console.Write("Title: ");
            note.Title = Console.ReadLine();

            Console.Write("Content: ");
            note.Content = Console.ReadLine();

            Console.Write("Note creation date (mm/dd/yyyy): "); ;
            note.CreateDateTime = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Choose a Post: ");

                for (int i = 0; i < posts.Count; i++)
                {
                    Post post = posts[i];
                    Console.WriteLine($"{post.Id} - {post.Title}");
                }
            note.PostId = int.Parse(Console.ReadLine());



            _noteRepository.Insert(note);
        }

        private Note Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Note:";
            }

            Console.WriteLine(prompt);

            List<Note> notes = _noteRepository.GetAll();

            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                Console.WriteLine($" {i + 1}) {note.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return notes[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Remove()
        {
            Note noteToDelete = Choose("Which Note would you like to remove?");
            if (noteToDelete != null)
            {
                _noteRepository.Delete(noteToDelete.Id);
            }
        }

        private void Edit()
        {
            Note noteToEdit = Choose("Which note would you like to edit?");
            if (noteToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                noteToEdit.Title = title;
            }
            Console.Write("New URL (blank to leave unchanged: ");
            string content = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(content))
            {
                noteToEdit.Content = content;
            }

            _noteRepository.Update(noteToEdit);
        }
    }
}
