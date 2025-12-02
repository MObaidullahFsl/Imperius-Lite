using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEngine;
using Imperius.Logic;

namespace Imperius.Data
{
    public class LoginDL
    {
        public LoginDL()
        {
            Debug.Log("Greeting from Login DL");
        }
        public async Task<User> verifyUserFromDB(UserDTO u)
        {   
            // do await Task.Yield(); on the web request
            var user = new User(1, u.username);
            Debug.Log("Dl user made :" +user);
            return user;
        }

    }
}

