package com.remo.features.Root;

import java.io.IOException;

public class Shell {
    private Process process;
    private void runShellCommand(String command) throws IOException, InterruptedException {
        process = Runtime.getRuntime().exec(command);
        process.waitFor();
    }
}
