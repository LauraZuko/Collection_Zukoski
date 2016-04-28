/*
Laura Zukoski
CS245 - OOP
Homework 8-Collections
This program creates a collection of books(objects)
It prompts the user to add an item to the collection,
which can then be removed, saved to binary/xml and read
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;


namespace Collection
{
    public class Manager
    {
        private BookIO io;
        private List<Book> askerBook;
        public Manager()
        {
            io = new BookIO();
            askerBook = new List<Book>();
        }
        public void run() { 
            int choiceNum = 0;
            do
            {
                // Prints menu choices
                Console.WriteLine("Select the number of your choice:");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Remove a book");
                Console.WriteLine("3. List Books");
                Console.WriteLine("4. Save Books");
                Console.WriteLine("5. Read books from file");
                Console.WriteLine("6. Quit");
                Console.Write("Enter the number of your choice: ");
                string choice = Console.ReadLine();
                BookIO list = new BookIO();
                BookIO ask = new BookIO();

                // if the number entered is not a valid choice, asks user to re-enter choice
                // for each valid choice, calls the corresponding action for choice
                try
                {
                    choiceNum = int.Parse(choice);
                    if (choiceNum == 1)
                    {
                        Book book = ask.AskUser();
                        askerBook.Add(book);
                    }
                    else if (choiceNum == 2)
                    {
                        list.ListToScreen(askerBook);
                        Console.Write("Enter the title of the book to delete: ");
                        string delete = Console.ReadLine();
                        var item = askerBook.First(x => x.Title == delete);    // finds matching title object from list and removes it
                        askerBook.Remove(item);
                    }
                    else if (choiceNum == 3)
                    {
                        list.ListToScreen(askerBook);
                    }
                    else if (choiceNum == 4)
                    {
                        Console.WriteLine("Enter the full path of the file, including .bin or .xml extension: ");
                        string fname = Console.ReadLine();

                        string type = fname.Substring(fname.Length - 4);
                        if (type == ".bin")
                        {
                            ask.WriteToBinary(askerBook, fname);
                        }
                        else if (type == ".xml")
                        {
                            ask.WriteXML(askerBook, fname);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Type");
                        }
                    }
                    else if (choiceNum == 5)
                    {
                        Console.WriteLine("Enter the full path of the file, including .bin or .xml extension: ");
                        string fname = Console.ReadLine();
                        // @"C:\Users\guest no log in\Dropbox\CS 245\Collection\data.bin"
                        string type = fname.Substring(fname.Length - 4);
                        if (type == ".bin")
                        {
                            ask.ReadFromBinary(askerBook, fname);
                        }
                        else if (type == ".xml")
                        {
                            ask.ReadXML(askerBook, fname);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Type");
                        }
                    }
                    else
                    {
                        Console.Write("Enter the number of your choice: ");
                        choice = Console.ReadLine();
                    }                   
                }
                catch (FormatException)
                {
                    Console.Write("Enter the number of your choice: ");
                    choice = Console.ReadLine();
                }
            } while (choiceNum != 6);
        }
    }
    [Serializable]
    public class Book
    {
        private string title;
        private string genre;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
        public string Genre
        {
            get
            {
                return genre;
            }
            set
            {
                genre = value;
            }
        }
        public Book()
        {
            title = "";
            genre = "";
        }
        public Book(string title, string genre)
        {
            this.title = title;
            this.genre = genre;
        }
        // Format to print the object to console
        public override string ToString()
        {
            return string.Format("Book, Title = {0}, Genre = {1}", title, genre);
        }
    }
    public class BookIO
    {
        // Asks User to enter a title and genre for Book object
        // Creates a new Book object each time and returns object
        public Book AskUser()
        {
            Console.Write("Enter a Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter a Genre: ");
            string genre = Console.ReadLine();
            Book b = new Book(title, genre);
            return (b);
        }
        // Prints list of objects to console by calling ToString function
        public void ListToScreen(List<Book> askerBook)
        {
            foreach (Book b in askerBook)
            {
                Console.WriteLine(b.ToString());
            }
        }
        // Writes Binary file
        public void WriteToBinary(List<Book> askerBook, string fname)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = File.Create(fname);
            bf.Serialize(stream, askerBook);
            stream.Close();
        }
        // Reads Binary file
        public void ReadFromBinary(List<Book> askerBook, string fname)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream st = File.OpenRead(fname);
            List<Book> result = (List<Book>)bf.Deserialize(st);
            Console.WriteLine(result);
            st.Close();
        }
        // Writes XML file
        public void WriteXML(List<Book> askerBook, string fname)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Book>));
            StreamWriter sw = new StreamWriter(fname);
            xml.Serialize(sw, askerBook);
            sw.Close();
        }
        // Reads XML file
        public void ReadXML(List<Book> askerBook,string fname)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Book>));
            StreamReader sr = new StreamReader(fname);
            List<Book> c = (List<Book>)xml.Deserialize(sr);
            sr.Close();
        }
    }
    // Main program is used to call Manager class that handles everything
    class Program
    {
        static void Main(string[] args)
        {
            Manager man = new Manager();
            man.run();
        }
    }
}