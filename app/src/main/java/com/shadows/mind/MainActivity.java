package com.shadows.mind;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

public class MainActivity extends AppCompatActivity {

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
        setContentView(R.layout.activity_main);
        hideSystemUi();

        TextView title = findViewById(R.id.titleText);
        Button start = findViewById(R.id.startButton);
        Button story = findViewById(R.id.storyButton);
        Button settings = findViewById(R.id.settingsButton);
        Button exit = findViewById(R.id.exitButton);

        title.setAlpha(0f);
        title.animate().alpha(1f).setDuration(1200).start();

        start.setAlpha(0f);
        story.setAlpha(0f);
        settings.setAlpha(0f);
        exit.setAlpha(0f);

        start.animate().alpha(1f).translationY(0f).setDuration(900).setStartDelay(150).start();
        story.animate().alpha(1f).translationY(0f).setDuration(900).setStartDelay(300).start();
        settings.animate().alpha(1f).translationY(0f).setDuration(900).setStartDelay(450).start();
        exit.animate().alpha(1f).translationY(0f).setDuration(900).setStartDelay(600).start();

        start.setOnClickListener(v -> startActivity(new Intent(this, GameActivity.class)));
        story.setOnClickListener(v -> startActivity(new Intent(this, StoryActivity.class)));
        settings.setOnClickListener(v -> startActivity(new Intent(this, SettingsActivity.class)));
        exit.setOnClickListener(v -> finishAffinity());
    }

    @Override
    protected void onResume() {
        super.onResume();
        hideSystemUi();
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        if (hasFocus) {
            hideSystemUi();
        }
    }
}
