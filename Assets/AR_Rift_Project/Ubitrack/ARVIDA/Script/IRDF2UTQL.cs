using UnityEngine;
using System.Collections;
using VDS.RDF;

public interface IRDF2UTQL  {

    string rdf2utql(IGraph graph);
	
}
