# Vouch Reputation System

Table of contents

1. [Introduction](#introduction)
2. [Vouching](#vouching)
3. [Reputation Score](#reputation)
4. [Security Concerns](#security)
    1. [Impersonation Attacks](#impersonation)
    2. [Vouch Forgery](#vouch_forgery)
    3. [Vouch Omission](#vouch_omission)

## Introduction <a name="introduction"></a>

Vouch Reputation System, or VRS for short, is a proof of concept for a decentralized reputation system for peer-to-peer networks. This particular implementation is written in Python.

The system's goal is to be a community-focused solution to the uncertainty of trust between unfamiliar peers in environments without authorities. Building upon already existing social bonds between its participants, VRS extrapolates a numerical reputation score for each node in the network in a way that reflects its standing on it.

In particular, VRS is suited for contexts where [complete]

## Vouching <a name="vouching"></a>

In VRS, every entity (person, organization, etc.) is represented by a node. Nodes can vouch for or against any other, signalizing trust and mistrust respectively. Vouches can be retracted and altered at will. A node's vouches (both for and against) are public to all.

Vouches are stored in what's called a vouch history. It's an ordered sequence of vouches and their logical timestamp in a way that preserves their past and current vouches.

Take for example Alice's vouch history:

| Logical Clock | Node    | Vouch   |
|---------------|---------|---------|
| 1             | Bob     | For     |
| 2             | Charlie | For     |
| 3             | Bob     | Against |

She currently vouches for Charlie and against Bob, but previously vouched for Bob.

Vouch connections are created between nodes depending on their vouches. If two nodes reciprocally vouch for the other, a vouch for connection is formed. If at least one of them vouches against the other, a vouch against connection is formed.

As such, the entities and their vouch connections can be displayed as a diagram of a network of nodes. When we are looking at the network from the perspective of a particular node, we call it the observer node.

The reputation of nodes in the network (including those the observer does not vouch for or against) can be assessed through the vouch connections there present.  

It's important to note that a node's reputation is not an intrinsic property it posesses but rather is contingent to an observer. The same node can have wildly different reputation scores when seen through the perspective of nodes with different vouch connections.

In concrete terms, let's say that Alice vouches for Bob. That means that Alice trusts him and is willing to put her own reputation on the line for him. If you yourself vouches for Alice but does not know Bob, you can reasonably assume that Bob is somewhat trustworthy based on his relationship with Alice. Reversely, Bob would seem untrustworthy to you had you vouched against Alice. Things would be more complicated were you connected to multiple nodes with different opinions on Bob, but the same logic applies.

Vouching is also a way for a node to make a public statement about some other's reputation while leaving it's own reputation as a collateral. That is to say, when a node vouches for some other it is essentially tying it's own reputation with the other's.

For instance, let's say you and Alice vouch for each other. If Bob were to distrust Alice and not know you, your own reputation (from his point of view) would be negatively affected by your association with Alice. Reversely, if Charlie does not know you but trusts Alice, your reputation would rise from his perspective. As before, things get more complex when the reputation of a node is affected by multiple sources.

Nodes may be connected to the observer by a varying degree of proximity. Naturally, the opinions of nodes closely connected to the observer carry more weight than those of distant ones. It's more impactful to be vouched by someone closer to the observer than by a distant connection.

Not only that, but the relevance of a node's opinion is also proportional to it's own reputation score. Let's say that Alice vouches for Bob and Charlie vouches against Bob, but Alice has a higher reputation score than Charlie. As Alice has a better standing on your network, Bob's reputation should be biased towards a good reputation score, rather than to be perfectly even between good and bad.

An observer can also apply a weight to every node. Weights serve as a multiplier for the node's opinion's relevance on the observer's network. For instance, even though you vouch for both Alice and Bob, you can make it so Bob's opinions are twice as relevant as Alice's by giving him a 2x weight while leaving Alice with the default 1x weight. Weights are not public information and can be kept private.

Finally, nodes that are deemed too loosely connected (or not connected at all) to the observer have no reputation score, as there is not enough meaningful data to estimate it. As such, they can be ommited from the network as far as the observer is concerned.

## Reputation Score <a name="reputation"></a>

## Security Concerns <a name="security"></a>

### Impersonation Attacks <a name="impersonation"></a>

One way an adversary might attack the system is by impersonating another user. Let's explore ways this attack vector can be mitigated.

Every entity should possess a asymmetric key pair as a means of authentication. In fact, the public key of this key pair is what identifies a node in the network. Every message created by a node (e.g. vouch messages) is to be signed with their private key. As such, nodes can verify the authorship of received messages through the means of digital signatures. Messages with missing or otherwise invalid signatures should be dismissed.

Asymmetric cryptography solves the issue of impersonating an entity's in-system identity. However, it does nothing to address the matter of associating a real life identity to a in-system identity.

One way this can be solved it through certificates, perhaps in a decentralized fashion like in [PGP](https://en.wikipedia.org/wiki/Pretty_Good_Privacy#Certificates), where nodes can certify others of their real identity and those that trust them can choose to listen. There might also be scenarios in which tying real life identities to a in-system identity is undesirable and anonymity is preferred.

### Vouch Forgery <a name="vouch_forgery"></a>

As discussed previously, vouch messages ought to be signed by their issuer. As such, it's not possible to forge a vouch message without knowing the would be victim's private key, which of course should be kept secret.

### Vouch Omission <a name="vouch_omission"></a>

Say that Alice vouches for Bob. She creates a message vouching for Bob which is forwarded along the network. After some time, Bob behaves in some way that warrents disapproval by others in the network. Many users vouch against Bob, which significantly decreases his overall reputation. This, in turn, means that Alice is also negatively affected with their association with Bob so she chooses to retract her vouch.

For overall network health, it's important that users are held accountable not only for their current vouch relations, but their past ones as well. Otherwise, bad actors could band together without repercussion, as if one of them gets caught the rest can simply retract their vouches and be unaffected. Instead, a user's past vouches should play some role in it's reputation calculation.

Now let's say that a new user, Charlie, enters the network for the first time after the Bob fiasco. As he doesn't know anything yet, he needs to ask around for the vouches each node has in order to construct the trust topology. When asked to present her vouch history, Alice might be inclined to not let Charlie know about her past association with Bob as a way of increasing her reputation. If she sends a tampered version of her vouch history, her reputation from Charlie's point of view may be artificially higher, misleading Charlie to trust Alice more than he might have had he known the full picture.

To prevent this scenario from happening, the first step is to make it so nodes don't just store the current vouch connections of the nodes they care about, but also have their previous vouches backed up. This can't be enforced, as users can always clean their cache to save disk space, but as long as at least one node in the victim's reach has the ommited message backed up the system can prevent vouch omission attacks.

Secondly, one should never ask a particular node about their vouch history. Instead, they should ask publicly about it so that anyone can chime in. This way, Charlie would ask anyone to provide for vouch messages made by Alice. Even if Alice omits her message vouching for Bob, another user that has it cached can interject and supply Charlie with the missing message. Alice can't repudiate the authenticity of the message as it contains a digital signature signed by her.
