package com.remo.Features.Root;

public class Shell {
    private Process process;
    private void runShellCommand(String command) throws Exception {
        process = Runtime.getRuntime().exec(command);
        process.waitFor();
    }
}
