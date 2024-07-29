package co.ab180.airbridge.unity;

import android.content.Intent;
import android.net.Uri;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Map;

import co.ab180.airbridge.Airbridge;
import co.ab180.airbridge.AirbridgeCallback;
import co.ab180.airbridge.event.Event;

public class AirbridgeUnity {

    private static String startDeeplink;
    private static String deeplinkCallbackObjectName;
    
    private static String receivedAttributionResult;
    private static String attributionResultCallbackObjectName;

    public static void startTracking() {
        Airbridge.startTracking();
    }
    
    public static void stopTracking() {
        Airbridge.stopTracking();
    }

    public static void setUserId(String id) {
        Airbridge.getCurrentUser().setId(id);
    }

    public static void setUserEmail(String email) {
        Airbridge.getCurrentUser().setEmail(email);
    }

    public static void setUserPhone(String phone) {
        Airbridge.getCurrentUser().setPhone(phone);
    }

    public static void setUserAlias(String key, String value) {
        if (key == null || value == null) return;
        Airbridge.getCurrentUser().setAlias(key, value);
    }

    public static void setUserAttribute(String key, int value) {
        if (key == null) return;
        Airbridge.getCurrentUser().setAttribute(key, value);
    }

    public static void setUserAttribute(String key, long value) {
        if (key == null) return;
        Airbridge.getCurrentUser().setAttribute(key, value);
    }

    public static void setUserAttribute(String key, float value) {
        if (key == null) return;
        Airbridge.getCurrentUser().setAttribute(key, value);
    }

    public static void setUserAttribute(String key, boolean value) {
        if (key == null) return;
        Airbridge.getCurrentUser().setAttribute(key, value);
    }

    public static void setUserAttribute(String key, String value) {
        if (key == null || value == null) return;
        Airbridge.getCurrentUser().setAttribute(key, value);
    }

    public static void clearUserAttributes() {
        Airbridge.getCurrentUser().clearAttributes();
    }

    public static void expireUser() {
        Airbridge.expireUser();
    }

    public static void clickTrackingLink(String trackingLink) {
        if (trackingLink == null) return;
        Airbridge.click(trackingLink, null);
    }

    public static void impressionTrackingLink(String trackingLink) {
        if (trackingLink == null) return;
        Airbridge.impression(trackingLink);
    }

    public static void trackEvent(String jsonString) {
        try {
            JSONObject object = new JSONObject(jsonString);
            Event event = AirbridgeEventParser.from(object);
            Airbridge.trackEvent(event);
        } catch (Exception e) {
            Log.e("AirbridgeUnity", "Error occurs while parsing data json string", e);
        }
    }

    public static void setDeeplinkCallback(String objectName) {
        deeplinkCallbackObjectName = objectName;
        if (startDeeplink != null && !startDeeplink.isEmpty()) {
            UnityPlayer.UnitySendMessage(deeplinkCallbackObjectName, "OnTrackingLinkResponse", startDeeplink);
            startDeeplink = null;
        }
    }

    @SuppressWarnings("WeakerAccess")
    public static void processDeeplinkData(Intent intent) {
        Airbridge.getDeeplink(intent, new AirbridgeCallback.SimpleCallback<Uri>() {

            @Override
            public void onSuccess(Uri uri) {
                if (deeplinkCallbackObjectName != null && !deeplinkCallbackObjectName.isEmpty()) {
                    UnityPlayer.UnitySendMessage(deeplinkCallbackObjectName, "OnTrackingLinkResponse", uri.toString());
                    startDeeplink = null;
                } 
                else { 
                    startDeeplink = uri.toString(); 
                }
            }

            @Override
            public void onFailure(@NotNull Throwable throwable) {

            }
        });
    }
    
    public static void setAttributionResultCallback(String objectName) {
        attributionResultCallbackObjectName = objectName;
        if (receivedAttributionResult != null && !receivedAttributionResult.isEmpty()) {
            UnityPlayer.UnitySendMessage(attributionResultCallbackObjectName, "OnAttributionResultReceived", receivedAttributionResult);
            receivedAttributionResult = null;
        }
    }

    public static void processAttributionData(Map<String, String> result) {
        if (result == null) return;

        JSONObject jsonObject = new JSONObject();
        try {
            for (Map.Entry<String, String> entry : result.entrySet()) {
                jsonObject.put(entry.getKey(), entry.getValue());
            }
            receivedAttributionResult = jsonObject.toString();

            if (attributionResultCallbackObjectName != null && !attributionResultCallbackObjectName.isEmpty()) {
                UnityPlayer.UnitySendMessage(attributionResultCallbackObjectName, "OnAttributionResultReceived", receivedAttributionResult);
                receivedAttributionResult = null;
            }
        } catch (JSONException e) {
            Log.e("AirbridgeUnity", "Error occurs while parsing attribution data to json string", e);
        }
    }
        
    public static void setDeviceAlias(String key, String value)
    {
        if (key == null || value == null) return;
        Airbridge.setDeviceAlias(key, value);
    }

    public static void removeDeviceAlias(String key)
    {
        if (key == null) return;
        Airbridge.removeDeviceAlias(key);
    }

    public static void clearDeviceAlias()
    {
        Airbridge.clearDeviceAlias();
    }
    
    public static void registerPushToken(String token)
    {
        if (token == null) return;
        Airbridge.registerPushToken(token);
    }
    
    public static boolean fetchAirbridgeGeneratedUUID(@NotNull String objectName) 
    {
        return Airbridge.fetchAirbridgeGeneratedUUID(uuid -> 
            UnityPlayer.UnitySendMessage(objectName, "OnFetchAirbridgeGeneratedUUID", uuid));
    }
    
    public static void startInAppPurchaseTracking() {
        Airbridge.startInAppPurchaseTracking();
    }
}