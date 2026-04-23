package com.shadows.mind;

import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

public class GameActivity extends AppCompatActivity {

    private final Handler handler = new Handler(Looper.getMainLooper());
    private View pulseLayer;
    private TextView statusText;

    private final Runnable pulseRunnable = new Runnable() {
        @Override
        public void run() {
            if (pulseLayer == null) {
                return;
            }

            float current = pulseLayer.getAlpha();
            float next = current < 0.18f ? 0.28f : 0.08f;
            pulseLayer.animate().alpha(next).setDuration(1400).start();

            if (statusText != null) {
                statusText.animate().alpha(next > 0.15f ? 1f : 0.75f).setDuration(700).start();
            }

            handler.postDelayed(this, 1600);
        }
    };

    private void hideSystemUi() {
        View decorView = getWindow().getDecorView();
        decorView.setSystemUiVisibility(
                View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY
                        | View.SYSTEM_UI_FLAG_FULLSCREEN
                        | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                        | View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                        | View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                        | View.SYSTEM_UI_FLAG_LAYOUT_STABLE
        );
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);
        hideSystemUi();

        pulseLayer = findViewById(R.id.pulseLayer);
        statusText = findViewById(R.id.statusText);

        pulseLayer.setAlpha(0.08f);
        handler.postDelayed(pulseRunnable, 900);

        findViewById(R.id.gameRoot).setOnClickListener(v -> {
            float a = pulseLayer.getAlpha();
            pulseLayer.animate().alpha(a > 0.15f ? 0.05f : 0.22f).setDuration(250).start();
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        hideSystemUi();
        handler.removeCallbacks(pulseRunnable);
        handler.postDelayed(pulseRunnable, 900);
    }

    @Override
    protected void onPause() {
        super.onPause();
        handler.removeCallbacks(pulseRunnable);
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        if (hasFocus) {
            hideSystemUi();
        }
    }
}
