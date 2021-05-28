using System;
using System.Collections.Generic;

//Vouch Reputation System by Bruno Rocha Moura
//https://github.com/BrunoRochaDev/

namespace VouchReputationSystem.Classes
{
    //The account in and of itself, before being representend in a personal network.
    //It holds the accounts information like name and vouches as well as handles vouch commands and checks logic.

    public class Account
    {
        //The account name, which has to be unique since the names are compared to determine if accounts are equal.
        public string name;

        //Dictionary with every node it has a connection with. True is vouch for, False is vouch against.
        public Dictionary<Account, bool> vouches = new Dictionary<Account, bool>();

        //Constructors
        public Account()
        {
            //Just here because of inheritance
        }
        public Account(string _name)
        {
            this.name = _name;
        }

        #region vouch

        //Creates a vouch for or against relation that is stored in this account's vouch dictionary.
        private void Vouch(Account _node, bool _polarity)
        {
            //Nodes cannot vouch for itself.
            if (_node.Equals(this))
            {
                Console.WriteLine("Error: Cannot vouch for itself.");
                return;
            }

            //If given node is already vouched for
            if (this.vouches.ContainsKey(_node))
            {
                //If the current vouch polarity is already what's being vouched, then remove it.
                if (vouches[_node].Equals(_polarity))
                    vouches.Remove(_node);
                //If not, then update it
                else
                    this.vouches[_node] = _polarity;
            }
            else
                //If not, vouch for it.
                this.vouches.Add(_node, _polarity);
        }
        //Simplified function only for vouching for
        public void VouchFor(Account _node)
        {
            this.Vouch(_node, true);
        }
        //Simplified function only for vouching against
        public void VouchAgainst(Account _node)
        {
            this.Vouch(_node, false);
        }

        public bool HasVouchForConnection(Account _otherNode)
        {
            /*
             A vouch for relations is only valid if it's reciprocal.
             */

            //Gets the vouch state from this node
            bool thisPolarity;
            if (this.vouches.ContainsKey(_otherNode))
                thisPolarity = this.vouches[_otherNode];
            else
                thisPolarity = false;

            //Gets the vouch state from the other node
            bool otherPolarity;
            if (_otherNode.vouches.ContainsKey(this))
                otherPolarity = _otherNode.vouches[this];
            else
                otherPolarity = false;

            //If both parties vouch for each other, then return true.
            return (thisPolarity && otherPolarity);
        }

        public bool HasVouchAgainstConnection(Account _otherNode)
        {
            //Gets the vouch state from this node
            bool thisPolarity = true;
            if (this.vouches.ContainsKey(_otherNode))
                thisPolarity = this.vouches[_otherNode];

            //Gets the vouch state from the other node
            bool otherPolarity = true;
            if (_otherNode.vouches.ContainsKey(this))
                otherPolarity = _otherNode.vouches[this];

            //If at least one of the parties vouch against the other, return true.
            return !(thisPolarity && otherPolarity);
        }

        #endregion

        //Overrides the ToString method so that it returns the account's name.
        public override string ToString()
        {
            return "name: " + this.name;
        }

        //Overrides the Equals method so that two accounts are seen as equal if they share the same name.
        public override bool Equals(object obj)
        {
            var item = obj as Account;

            if (item == null)
            {
                return false;
            }

            return this.name.Equals(item.name);
        }
        //Generate hash based only on the account's name.
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }
}
