Shader "Hidden/Shader/ScreenOverlay"
{
    HLSLINCLUDE

    #pragma target 4.5

    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    struct Attributes {
        uint vertexID : SV_VertexID;

        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings {
        float4 positionCS : SV_POSITION;

        float2 texcoord   : TEXCOORD0;

        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input) {
        Varyings output;

        UNITY_SETUP_INSTANCE_ID(input);

        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);

        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);

        return output;
    }

    // List of properties to control your post process effect

    TEXTURE2D_X(_InputTexture);

    float ScreenTransitionStart = -1;
    float ScreenTransitionSpeed = -1;

    float4 CustomPostProcess(Varyings input) : SV_Target {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        uint2 positionSS = input.texcoord * _ScreenSize.xy;

        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

        if(ScreenTransitionStart == -1) return float4(outColor, 1);

        float progress = saturate((_Time.y - ScreenTransitionStart)*abs(ScreenTransitionSpeed));
        
        if(ScreenTransitionSpeed < 0)
            progress = 1 - progress;
                
        outColor *= progress;

        return float4(outColor, 1);
    }

    ENDHLSL

    SubShader {
        Pass {
            Name "TransitionOverlay"

            ZWrite Off

            ZTest Always

            Blend Off

            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess

                #pragma vertex Vert
            ENDHLSL
        }
    }

    Fallback Off
}