class {:extern} Taxon {
    function {:extern} IsSubTaxon(potentialAncestor: Taxon): bool
}

class {:extern} Observation {
    function {:extern} GetTaxon(): Taxon
}

function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> {
    // Create an empty Observation sequence.
    // Run through every Observation in obs, and check if its taxon == root, or if a subtaxon is.
    // Concatenate each correct Observation to the new sequence.
    // Return the new sequence.
}