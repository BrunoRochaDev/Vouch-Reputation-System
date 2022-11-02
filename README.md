# Vouch Reputation System

## Introduction

Vouch Reputation System, or VRS for short, is a proof of concept for a decentralized reputation system for peer-to-peer networks written in Python.

It's goal is to be a community-focused solution to the uncertainty of trust between unfamiliar peers in environments without authorities. Building upon already existing social bonds between it's participants, VRS extrapolates a reputation score for each node in the network in a way that reflects it's standing on it.

## Vouching

In VRS, every entity (person, organization, etc.) is represented by a node. Nodes can vouch for or against any other, signalizing trust or mistrust. Vouches can be retracted and altered at will. A node's vouches (both for and against) is public to all other nodes.

Vouch connections are created between nodes depending on their vouches. If two nodes reciprocally vouch for the other, a vouch for connection is formed. If at least one of them vouches against the other, a vouch against connection is formed.

As such, all the entities and their vouch connections can be displayed as a diagram of a network of nodes.

It's important to note that a node's reputation is not an intrinsic property it posesses but rather is dependent to an observer. The same node can have wildly different reputation scores when seen through the perspective of nodes with vastly different vouch connections.

Through the vouch connections, we can estimate the reputation of nodes the observer does not know based on the connections of the nodes it does know. 

In concrete terms, let's say that Alice vouches for Bob. That means that Alice trusts him and is willing to put her own reputation on the line for him. If you yourself vouches for Alice but does not know Bob, you can reasonably assume that Bob is somewhat trustworthy based on it's relationship with Alice, whom you trust. Things get more complicated however if you were connected to multiple nodes with different opinions on Bob, but the same logic applies.

Vouching is also a way for a node to make a public statement about some other's reputation while leaving it's own reputation as a collateral. That is to say, when a node vouches for some other, it is essentially tying it's own reputation with the other's.

For instance, let's say you and Alice vouch for each other. If Bob were to distrust Alice and not know you, your own reputation (from his point of view) would be negatively affected by your association with Alice. Reversely, if Charlie does not know you but trusts Alice, your reputation would rise from his perspective. As before, things get more complex when the reputation of a node is affected by multiple sources.
