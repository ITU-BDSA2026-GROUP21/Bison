class {:extern} Taxon {
    function {:extern} IsSubTaxon(potentialAncestor: Taxon): bool
}

class {:extern} Observation {
    function {:extern} GetTaxon(): Taxon?
}

// Keep is a helper predicate to determine whether an Observation should be kept in the final returned sequence.
// It first checks if the Observation has a Taxon at all. If the taxon is null, 
// the predicate determines that it should not be kept.
// Then, in the else branch, the predicate checks if the Observation's taxon is either the same as the root, 
// or if it's a subtaxon of the root. If either of those are true, the predicate returns true.
predicate Keep(root: Taxon, obs: Observation) {
    var taxon := obs.GetTaxon();
    if taxon == null then false
    else taxon == root || taxon.IsSubTaxon(root)
}


// This function recursively filters out the Observations in the given sequence that don't have taxon that matches with the root.
// An Observation is kept if its taxon is either the same as the root, or that its taxon is a subtaxon of the root.
function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> 
    ensures forall o :: o in FilterBy(root, obs) ==> o in obs && Keep(root, o)
    ensures forall o :: o in obs && Keep(root, o) ==> o in FilterBy(root, obs)
{
    if |obs| == 0 then [] // If the given sequence of Observations is empty, return an empty sequence
    else if Keep(root, obs[0]) then [obs[0]] + FilterBy(root, obs[1..])
    else FilterBy(root, obs[1..])
}