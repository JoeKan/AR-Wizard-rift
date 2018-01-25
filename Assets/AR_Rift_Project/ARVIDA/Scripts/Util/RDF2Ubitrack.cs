using UnityEngine;
using System.Collections;
using FAR;

public class RDF2Ubitrack {

    public static ulong getTimeStamp(RDFTerm term)
    {
        return term["spatial:timestamp"]["vom:quantityValue"]["vom:numericalValue"].getLiteralValue<ulong>();
    }

    public static Measurement<FAR.Pose> getPose(RDFTerm term)
    {
        RDFTerm quaterion = term["spatial:rotation"]["vom:quantityValue"];
        float qx = quaterion["maths:x"].getLiteralValue<float>();
        float qy = quaterion["maths:y"].getLiteralValue<float>();
        float qz = quaterion["maths:z"].getLiteralValue<float>();
        float qw = quaterion["maths:w"].getLiteralValue<float>();

        Quaternion ubiQ = new Quaternion(qx, qy, qz, qw);
        Quaternion unityQ = new Quaternion();
        FAR.UbiMeasurementUtils.coordsysemChange(ubiQ, ref unityQ);

        RDFTerm translation = term["spatial:translation"]["vom:quantityValue"];
        float tx = translation["maths:x"].getLiteralValue<float>();
        float ty = translation["maths:y"].getLiteralValue<float>();
        float tz = translation["maths:z"].getLiteralValue<float>();


        Vector3 ubiT = new Vector3(tx, ty, tz);
        Vector3 unityT = new Vector3();
        UbiMeasurementUtils.coordsysemChange(ubiT, ref unityT);

        FAR.Pose pose = new FAR.Pose(unityT, unityQ);
        ulong ts = getTimeStamp(term);
        return new Measurement<FAR.Pose>(pose, ts);
    }

    public static Measurement<float[]> getMatrix3x3(RDFTerm term)
    {
         
        
        RDFTerm qValue = term["vom:quanity"]["vom:quantityValue"];
        float[] matrix3x3 = new float[9];
        matrix3x3[0] = qValue["maths:a11"].getLiteralValue<float>();
        matrix3x3[1] = qValue["maths:a12"].getLiteralValue<float>();
        matrix3x3[2] = qValue["maths:a13"].getLiteralValue<float>();

        matrix3x3[3] = qValue["maths:a21"].getLiteralValue<float>();
        matrix3x3[4] = qValue["maths:a22"].getLiteralValue<float>();
        matrix3x3[5] = qValue["maths:a23"].getLiteralValue<float>();

        matrix3x3[6] = qValue["maths:a31"].getLiteralValue<float>();
        matrix3x3[7] = qValue["maths:a32"].getLiteralValue<float>();
        matrix3x3[8] = qValue["maths:a33"].getLiteralValue<float>();
        

        
        ulong ts = getTimeStamp(term);
        return new Measurement<float[]>(matrix3x3, ts);
    }


}
