using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    class PostManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _connectionString = connectionString;
            _blogRepository = new BlogRepository(connectionString);
        }
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 6) Note Management");
            Console.WriteLine(" 0) Return to Main Menu");

            Console.Write("> ");
            string choice = Console.ReadLine();
            Console.Clear();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return this;
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "6":
                    throw new NotImplementedException();
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            Console.WriteLine("Posts: ");
            Console.WriteLine("");
            foreach (Post post in posts)
            {
                Console.WriteLine($"Title: {post.Title} - URL: {post.Url}");
            }
            Console.WriteLine("");
        }

        private void Add()
        {
            List<Author> authors = _authorRepository.GetAll();
            List<Blog> blogs = _blogRepository.GetAll();

            Author postAuthor = null;
            Blog postBlog = null;
            DateTime postDate = new DateTime();
            Console.Clear();
            string postTitle = StringPrompt("Enter the post title: ");
            Console.Clear();
            string postUrl = StringPrompt("Enter the post url: ");

            bool enteringDate = true;
            while (enteringDate)
            {
                Console.Write("Enter the post date. Use this formatting: (mm/dd/yyyy): ");
                string date = Console.ReadLine();
                try
                {
                    postDate = DateTime.Parse(date);
                    break;
                   
                }
                catch (Exception x)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Selection");
                }
            }

            bool enteringAuthor = true;
            while (enteringAuthor)
            {
                Console.WriteLine("Choose an Author: ");

                for (int i = 0; i < authors.Count; i++)
                {
                    Author author = authors[i];
                    Console.WriteLine($"{author.Id} - {author.FullName}");
                }
                string input = StringPrompt("> ");
                try
                {
                    Console.Clear();
                    int choice = int.Parse(input);
                    postAuthor = authors.Find(a => a.Id == choice);
                    break;
                }
                catch (Exception x)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Selection");
                }
            }

            bool enteringBlog = true;
            while (enteringBlog)
            {
                Console.WriteLine("Please choose the Post's Blog:");

                for (int i = 0; i < blogs.Count; i++)
                {
                    Blog blog = blogs[i];
                    Console.WriteLine($"{blog.Id} - {blog.Title}");
                }
                Console.Write("> ");
                string input = Console.ReadLine();
                try
                {
                    Console.Clear();
                    int choice = int.Parse(input);
                    postBlog = blogs.Find(b => b.Id == choice);
                    break;
                }
                catch (Exception x)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Selection");
                }
            }

            Post newPost = new Post()
            {
                Title = postTitle,
                Url = postUrl,
                Blog = postBlog,
                Author = postAuthor,
                PublishDateTime = postDate
            };

            _postRepository.Insert(newPost);

            Console.Clear();
            Console.WriteLine($"\"{newPost.Title}\" was successfully added!");
            Console.WriteLine();
        }
        private void Edit()
        {
            Console.Clear();


            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            string newTitle = StringPrompt("Enter a new title (If no change, just hit enter): ");
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                postToEdit.Title = newTitle;
            }

            string newUrl = StringPrompt("Enter a new URL (If no change, just hit enter): ");
            if (!string.IsNullOrWhiteSpace(newUrl))
            {
                postToEdit.Url = newUrl;
            }

            bool enteringDate = true;
            while (enteringDate)
            {
                DateTime newDate = new DateTime();
                string dateInput = StringPrompt("Enter a new post publication date (mm/dd/yyyy) (If no change, just hit enter): ");
                if (string.IsNullOrWhiteSpace(dateInput))
                {
                    break;
                }
                else
                {
                    try
                    {
                        newDate = DateTime.Parse(dateInput);
                        postToEdit.PublishDateTime = newDate;
                        break;
                    }
                    catch (Exception x)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid date");
                    }
                }
            }

            bool enteringAuthor = true;
            List<Author> authors = _authorRepository.GetAll();
            while (enteringAuthor)
            {
                Console.WriteLine("Please choose a new author (If no change, just hit enter): ");

                for (int i = 0; i < authors.Count; i++)
                {
                    Author author = authors[i];
                    Console.WriteLine($"{author.Id} - {author.FullName}");
                }

                string input = StringPrompt("> ");
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                else
                {
                    try
                    {
                        Console.Clear();
                        int choice = int.Parse(input);
                        postToEdit.Author = authors.Find(a => a.Id == choice);
                        break;
                    }
                    catch (Exception x)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Selection");
                    }
                }
            }

            bool enteringBlog = true;
            List<Blog> blogs = _blogRepository.GetAll();
            while (enteringBlog)
            {
                Console.WriteLine("Please choose a new blog (If no change, just hit enter): ");

                for (int i = 0; i < blogs.Count; i++)
                {
                    Blog blog = blogs[i];
                    Console.WriteLine($"{blog.Id} - {blog.Title}");
                }
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    enteringBlog = false;
                }
                else
                {

                    try
                    {
                        Console.Clear();
                        int choice = int.Parse(input);
                        postToEdit.Blog = blogs.Find(b => b.Id == choice);
                        enteringBlog = false;
                    }
                    catch (Exception x)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Selection");
                    }
                }
            }

            _postRepository.Update(postToEdit);
            Console.Clear();
            Console.WriteLine($"Post \"{postToEdit.Title}\" was successfully edited.");
            Console.WriteLine();
        }
        private void Remove()
        {
            Console.Clear();
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
            Console.Clear();
            Console.WriteLine($"{postToDelete.Title} was successfully removed.");
            Console.WriteLine();
        }
        private Post Choose(string prompt = null)
        {
            Console.Clear();
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                Console.Clear();
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception x)
            {
                Console.Clear();
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        /// <summary>
        /// prints the message parameter 
        /// as a prompt for the user, returns user input as a string
        /// </summary>
        private string StringPrompt(string message)
        {
            Console.Write(message);
            string output = Console.ReadLine();
            return output;
        }
    }
}