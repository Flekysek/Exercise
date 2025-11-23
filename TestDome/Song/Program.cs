using System;
using System.Collections.Generic;

public class Song
{
    private string name;
    public Song NextSong { get; set; }

    public Song(string name)
    {
        this.name = name;
    }
    
    public bool IsInRepeatingPlaylist()
    {
        var slow = this;
        var fast = this;

        while (fast != null && fast.NextSong != null)
        {
            slow = slow.NextSong;
            
            fast = fast.NextSong.NextSong;

            if (slow == fast)
            {
                return true;
            }
        }

        return false;
    }
    
    public static void Main(string[] args)
    {
        var firstA = new Song("Hello");
        var secondA = new Song("Eye of the tiger");
    
        firstA.NextSong = secondA;
        secondA.NextSong = firstA; 
    
        Console.WriteLine($"Příklad A (Cyklus): {firstA.IsInRepeatingPlaylist()}"); 

        var firstB = new Song("World");
        var secondB = new Song("End");
        var thirdB = new Song("...");
        
        firstB.NextSong = secondB;
        secondB.NextSong = thirdB;
        thirdB.NextSong = null; 
        
        Console.WriteLine($"Příklad B (Bez cyklu): {firstB.IsInRepeatingPlaylist()}"); 
    }
}