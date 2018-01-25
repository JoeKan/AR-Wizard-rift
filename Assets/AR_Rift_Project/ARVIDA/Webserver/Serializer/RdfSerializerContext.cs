using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;


    public class RdfSerializerContext
    {
        public Uri BaseUri { get; private set; }
        public Uri RelativeUri { get; private set; }
        


        public RdfSerializerContext(String baseUri, String relativeUri)
        {
            BaseUri = new Uri(baseUri);
            RelativeUri = new Uri(relativeUri, UriKind.Relative);
        }


    }

