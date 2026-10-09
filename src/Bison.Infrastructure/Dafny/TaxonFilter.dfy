class {:extern} Taxon {
    function {:extern} IsSubTaxon(potentialAncestor: Taxon): bool
}

class {:extern} Observation {
    function {:extern} getTaxon(): Taxon
}

function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>