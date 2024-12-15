Shader "Custom/WetFloorEmissiveBloom" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Floor Texture", 2D) = "white" {}
        _Tiling("Texture Tiling", Vector) = (1,1,0,0)
        _Offset("Texture Offset", Vector) = (0,0,0,0)
        _TextureStrength("Texture Strength", Range(0, 1)) = 0.7
        _Wetness("Wetness", Range(0, 1)) = 0.5
        _Reflectivity("Reflectivity", Range(0, 1)) = 0.5
        _FresnelPower("Fresnel Power", Range(1, 5)) = 5
        _EmissiveBloomIntensity("Emissive Bloom Intensity", Range(0, 5)) = 1
        _EmissiveBloomThreshold("Emissive Bloom Threshold", Range(0, 1)) = 0.5
    }
        SubShader{
            Tags {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows alpha:premul
            #pragma target 3.0
            #pragma multi_compile _PRID_ONE _PRID_TWO _PRID_THREE _PRID_FOUR
            #include "PlanarReflections.cginc"

            struct Input {
                float2 uv_MainTex;
                float4 screenPos;
                float3 viewDir;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _Tiling;
            float4 _Offset;
            half _TextureStrength;
            half _Wetness;
            half _Reflectivity;
            half _FresnelPower;
            half _EmissiveBloomIntensity;
            half _EmissiveBloomThreshold;

            float3 ApplyEmissiveBloom(float3 color) {
                float3 luminance = dot(color, float3(0.2126, 0.7152, 0.0722));
                float brightness = max(luminance.r, max(luminance.g, luminance.b));
                float contribution = max(0, brightness - _EmissiveBloomThreshold);
                return lerp(color, color * _EmissiveBloomIntensity, contribution);
            }

            void surf(Input IN, inout SurfaceOutputStandard o) {
                float2 uv = IN.uv_MainTex * _Tiling.xy + _Offset.xy;
                fixed4 c = tex2D(_MainTex, uv) * _Color;

                o.Metallic = 0;
                o.Smoothness = _Wetness;
                o.Normal = float3(0, 0, 1);

                half refl = _Reflectivity * _Wetness;
                half cos = saturate(dot(o.Normal, normalize(IN.viewDir)));
                refl = lerp(refl, 1, pow(1 - cos, _FresnelPower) * _Wetness);

                float3 reflColor = SamplePlanarReflections(IN.screenPos).rgb;
                float3 bloomedRefl = ApplyEmissiveBloom(reflColor);

                // 텍스처와 반사를 더 자연스럽게 블렌딩
                o.Albedo = lerp(c.rgb, c.rgb * (1 - refl), _Wetness);
                o.Alpha = lerp(_TextureStrength, 1, _Wetness);
                o.Emission = bloomedRefl * refl;
            }
            ENDCG
        }
            FallBack "Diffuse"
}