# Shadow in My Mind
> *"You were never alone in your mind."*

First-person psychological horror — Android — Unity 2022.3 LTS + URP

## Systems

| Script | Role |
|--------|------|
| `GameManager` | Global state machine |
| `SanitySystem` | Drives all horror intensity |
| `ShadowController` | AI: Dormant → Observing → Stalking → Chasing → Breakdown |
| `PostProcessingController` | URP FX tied to sanity |
| `AudioManager` | Whispers fade in as sanity drops |
| `UIManager` | VHS overlay + minimal HUD |
| `MemoryFragment` | Collectibles that drain sanity |
| `ShadowMimic` | Shadow replaces environment objects |
| `LightZone` | Safe areas that recover sanity |

## Quick Start (Termux)

```bash
bash setup-shadow.sh
cd ShadowInMyMind
git init
git remote add origin https://github.com/YOUR_USER/shadow-in-my-mind.git
git add .
git commit -m "init: Shadow in My Mind scaffold"
git push -u origin main
# GitHub Actions builds the .apk automatically
```

## GitHub Secrets Required

| Secret | Value |
|--------|-------|
| `UNITY_LICENSE` | XML from unity-activate |
| `UNITY_EMAIL` | Unity account email |
| `UNITY_PASSWORD` | Unity account password |
| `ANDROID_KEYSTORE_BASE64` | base64 of your .keystore |
| `ANDROID_KEYSTORE_NAME` | keystore filename |
| `ANDROID_KEYSTORE_PASS` | keystore password |
| `ANDROID_KEYALIAS_NAME` | key alias |
| `ANDROID_KEYALIAS_PASS` | key alias password |

## Generate Keystore (requires JDK)

```bash
keytool -genkey -v \
  -keystore shadow.keystore \
  -alias shadowinmymind \
  -keyalg RSA -keysize 2048 -validity 10000

# Encode for GitHub Secret:
base64 shadow.keystore | tr -d '\n'
```

---
**KraqCO** — Shadow in My Mind
