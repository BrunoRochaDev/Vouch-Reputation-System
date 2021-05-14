using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VouchReputationSystem
{
    class AccountChain
    {
        public string name;
        public int id;
        static int id_count = 0;

        //Dictionary with every node it has a connection with. True is vouch for, False is vouch against.
        public Dictionary<AccountChain, bool> connections = new Dictionary<AccountChain, bool>();
        
        //Constructors
        public AccountChain()
        {
            //Just here because of inheritance
        }
        public AccountChain(string _name)
        {
            this.name = _name;
            this.id = ++id_count;
        }

        #region vouch

        //Function that adds a node to the edge dictionary.
        public void Vouch(AccountChain _node, bool _polarity)
        {
            //Cannot add itself as a vouch
            if (_node.Equals(this))
            {
                Console.WriteLine("Error: Cannot vouch for itself.");
                return;
            }

            //If given node is already vouched for
            if (this.connections.ContainsKey(_node))
            {
                //If the current vouch polarity is already whats being vouched, then remove it
                if (connections[_node].Equals(_polarity))
                    connections.Remove(_node);
                //If not, then update it
                else
                    this.connections[_node] = _polarity;
            }
            else
                //If not, vouch for it.
                this.connections.Add(_node, _polarity);
        }
        //Simplified function only for vouching for
        public void VouchFor(AccountChain _node)
        {
            this.Vouch(_node, true);
        }
        //Simplified function only for vouching against
        public void VouchAgainst(AccountChain _node)
        {
            Console.WriteLine(this.name + " vouched against " + _node.name);
            this.Vouch(_node, false);
        }

        public bool DoesVouchFor(AccountChain _otherNode)
        {
            //For now, people can only vouch for someone if they are being vouched for in return.

            //Gets the vouch state from this node
            bool thisPolarity;
            if (this.connections.ContainsKey(_otherNode))
                thisPolarity = this.connections[_otherNode];
            else
                thisPolarity = false;

            //Gets the vouch state from the other node
            Console.WriteLine("Connections " +_otherNode.name);
            foreach (AccountChain _node in _otherNode.connections.Keys)
                Console.WriteLine(_node.ToString());
            bool otherPolarity;
            if (_otherNode.connections.ContainsKey(this))
                otherPolarity = _otherNode.connections[this];
            else
                otherPolarity = false;

            return (thisPolarity && otherPolarity);
        }

        public bool DoesVouchAgainst(AccountChain _otherNode)
        {
            //Gets the vouch state from this node
            bool thisPolarity = true;
            if (this.connections.ContainsKey(_otherNode))
                thisPolarity = this.connections[_otherNode];

            //Gets the vouch state from the other node
            bool otherPolarity = true;
            if (_otherNode.connections.ContainsKey(this))
                otherPolarity = _otherNode.connections[this];

            return !(thisPolarity && otherPolarity);
        }

        #endregion

        #region string-functions
        //Print object
        public override string ToString()
        {
            return "name: " + this.name;
        }
        #endregion

        //Equivalence
        public override bool Equals(object obj)
        {
            var item = obj as AccountChain;

            if (item == null)
            {
                return false;
            }

            return this.id.Equals(item.id);
        }
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    }
}
