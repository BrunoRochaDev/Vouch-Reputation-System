# Vouch Reputation System #
Vouch Reputation System, or VRS for short, is a decentralized reputation system for P2P networks written in C#.

It's goal is to be a community-focused solution to the uncertainty of trust between unfamiliar peers in environments without authorities, building upon the already existing social bonds between it's participants.

In VRS, every account in the network can vouch for or against any other, signalizing trust or mistrust. As such, all the different accounts and their vouch relations can be displayed as a diagram of a network of nodes.

<p align="center">
  <img src="https://user-images.githubusercontent.com/44985385/118741349-70754880-b824-11eb-8db5-f3136d270a5c.png">
</p>

<p align="center">
  <i>Vouch for relations are in green, vouch against relations in red.</i>
</p>

A vouch for relation is only formed if it's reciprocal between both parties. In contrast, vouch against relations are formed even if only one the parties vouch against the other.

A node can then be selected to be the observer node, creating a visualization of a personal network comprising of itself and the nodes it connects to (even tangentially). From that, each node in it's personal network is given a reputation score between zero and one. A node's reputation score is the reflection of the network's trust on it.

TODO explain the reputation algorithm

It's important to note that reputation scores are a subjective measure dependent on an observer and it's preferences and not an objective property of a node.

Through VRS, a user can assess anyone's (even complete strangers) trustworthiness through their reputation score as long as someone is their personal network is connected to them.

From this set of rules emerges an entropic force that promotes network safety. Vouching in favor of an account which is generally considered to be untrustworthy will bring your own reputation score down to those that vouch against it. Reversely, vouching against a reputable account will also cause the same effect to those that vouch for it. Users that don't vouch for or against others when it becomes warrented will lose out on reputation gains, potentially new connections and a more accurate reputation estimate in their own personal network.

This means that non-malicious users looking to maximize their own experience in the network are incentivized to be careful in regards to whom they associate with and be vigilant about troublemakers in their own personal network. Furthermore, ill-intentioned users are incentivized to act independently and not form connections between each other, as being associated with known bad actors will bring down their own reputation score, severally hampering coordination between malefactors.

In theory, VRS ensures that a perfectly healthy social network comprised of self interested actors with flawless judgement would eventually divide itself between a network of reputable users and a loose assortment of non-reputable users, but such conditions are not likely to occur in reality. In practice, as long as benign users are more numerous and well connected than hostile users, it will not be feasible for malicious actors to infiltrate a cautious node's personal network and cause harm.

In conclusion, the system proposed presents it's users with a personalized numeric representation for trust for every member of their personal network, extrapolated from their own preexisting social bonds. Through the mechanism of 'the discipline of continuous dealings' promoted by the system rules, users are incentivized to be consistent in their good behaviour and to ostracize actors that might compromise network safety, all for their own immediate self interest. VRS is modular and highly customizable and as such can be integrated into a variety of environments.
