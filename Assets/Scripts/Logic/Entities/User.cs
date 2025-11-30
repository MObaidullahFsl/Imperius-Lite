using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Imperius.Logic;


namespace Imperius.Logic
{
    /// <summary>
    /// is the login class, parent of player or admin 
    /// </summary>
    [System.Serializable]
    public class User
    {
        int _id; 
        string _username;
        // string password; prolly better to not store password anywhere except db encrypted
        
        // string role;  player || admin etc 

        public User( int id, string username)
        {
             _id = id;
             _username = username;

        }
        public User()
        {}

        public int Id { get => _id; set => _id = value; }
        public string Username { get => _username; set => Username = value; }
    }

}