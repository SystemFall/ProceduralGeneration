Shader "Custom/Terrain"
{
    Properties
    {
        //testTexture("Texture", 2D) = "black"{}
        //testScale("Scale", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        //const static int maxColourCount = 8;
        //const static float epsilon = 1E-4;

        //int baseColourCount;
        //float3 baseColours[maxColourCount];
        //float baseStartHeights[maxColourCount];
        //float baseBlends[maxColourCount];

        //float minHeight;
        //float maxHeight;

        struct Input {
            float3 worldPos;
        };

        /*float inverseLerp(float a, float b, float value) {
            return saturate((value - a) / (b - a));
        }*/

        void surf(Input IN, inout SurfaceOutputStandard o) {
            /*float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            for (int i = 0; i < baseColourCount; i++) {
                float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercent - baseStartHeight[i]);
                o.Albedo = o.Albedo * (1 - drawStrength) + baseColours[i] * drawStrength;
            }*/

            //o.Albedo = tex2D(testTexture, IN.worldPos.xz / testScale);

            o.Albedo = float3(0, 1, 0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
