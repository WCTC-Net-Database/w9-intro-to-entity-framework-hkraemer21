using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"'{name}' added to the game.");
    }

    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();

        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine());

        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());

        // TODO Add character to the room
        // Find the room by ID
        var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
        // If the room doesn't exist, return
        if (room == null)
        {
            Console.WriteLine("Room not found.");
            return;
        }
        // Otherwise, create a new character and add it to the room
        var character = new Character
        {
            Name = name,
            Level = level,
            RoomId = roomId
        };
        Console.WriteLine($"'{name}' added to the game.");
        // Save the changes to the database
        _context.SaveChanges();

    }

    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        // TODO Find the character by name
        // Use LINQ to query the database for the character
        // ignoring case sensitivity
        var character = _context.Characters.FirstOrDefault(c => c.Name.Contains(name));
        // If the character exists, display the character's information
        if (character != null)
        {
            Console.WriteLine($"Character found: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
        }
        // Otherwise, display a message indicating the character was not found
        else
        {
            Console.WriteLine("Character not found.");
            return;
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void UpdateCharacterLevel()
    {
        Console.Write("Enter character name to update: ");
        var name = Console.ReadLine();
        name = name.ToUpper();
        var character = _context.Characters.FirstOrDefault(c => c.Name.Contains(name));
        if (character != null)
        {
            Console.Write("Enter new level: ");
            var newLevel = int.Parse(Console.ReadLine());
            character.Level = newLevel;
            _context.SaveChanges();
            Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
        }
        else
        {
            Console.WriteLine("Character not found.");
            return;
        }

    }
}