#!/bin/bash

# Author: Authmane Terki (authmane512)
# E-mail: authmane512 (at) protonmail.ch
# Blog: https://medium.com/@authmane512
# Source: https://github.com/authmane512/android-project-template
# Tutorial: https://medium.com/@authmane512/how-to-do-android-development-faster-without-gradle-9046b8c1cf68
# This project is on public domain
#
# Hello! I've made this little script that allow you to init, compile and run an Android Project.
# I tried to make it as simple as possible to allow you to understand and modify it easily.
# If you think there is a very important missing feature, don't hesitate to do a pull request on Github and I will answer quickly.
# Thanks! 
 
set -e

APP_NAME="Remo"
PACKAGE_NAME="com.remo"
BUILD_TOOLS_VERSION=27.0.3
ANDROID_VERSION=19

AAPT="${ANDROID_SDK_ROOT}/build-tools/${BUILD_TOOLS_VERSION}/aapt"
DX="${ANDROID_SDK_ROOT}/build-tools/${BUILD_TOOLS_VERSION}/dx.bat"
ZIPALIGN="${ANDROID_SDK_ROOT}/build-tools/${BUILD_TOOLS_VERSION}/zipalign"
APKSIGNER="${ANDROID_SDK_ROOT}/build-tools/${BUILD_TOOLS_VERSION}/apksigner.bat"
PLATFORM="${ANDROID_SDK_ROOT}/platforms/android-${ANDROID_VERSION}/android.jar"
ADB="${ANDROID_SDK_ROOT}/platform-tools/adb"

init() {
	# rm -rf .git README.md
	echo "Making ${PACKAGE_NAME}..."
	mkdir -p "$PACKAGE_DIR"
	# rm -rf obj
	mkdir obj
	# rm -rf bin
	mkdir bin
	# rm -rf res/layout
	mkdir -p res/layout
	# rm -rf res/values
	mkdir res/values
	# rm -rf res/drawable
	mkdir res/drawable
	
	sed "s/{{ PACKAGE_NAME }}/${PACKAGE_NAME}/" "template_files/MainActivity.java" > "$PACKAGE_DIR/MainActivity.java"
	sed "s/{{ PACKAGE_NAME }}/${PACKAGE_NAME}/" "template_files/AndroidManifest.xml" > "AndroidManifest.xml"
	sed "s/{{ APP_NAME }}/${APP_NAME}/" "template_files/strings.xml" > "res/values/strings.xml"
	cp "template_files/activity_main.xml" "res/layout/activity_main.xml"
	# rm -rf template_files
}

init1() {
	echo "Making ${PACKAGE_NAME}..."
	sed "s/{{ PACKAGE_NAME }}/${PACKAGE_NAME}/" "template_files/MainActivity.java" > "$PACKAGE_DIR/MainActivity.java"
	sed "s/{{ PACKAGE_NAME }}/${PACKAGE_NAME}/" "template_files/AndroidManifest.xml" > "AndroidManifest.xml"
	sed "s/{{ APP_NAME }}/${APP_NAME}/" "template_files/strings.xml" > "res/values/strings.xml"
	cp "template_files/activity_main.xml" "res/layout/activity_main.xml"
	# rm -rf template_files
}

build() {
	echo "Stopping App..."
	$ADB shell pm clear $PACKAGE_NAME
	echo "Cleaning..."
	rm -rf obj/*
	rm -rf "$PACKAGE_DIR/R.java"

	echo "Generating R.java file..."
	$AAPT package -f -m -J src -M AndroidManifest.xml -S res -I $PLATFORM

	echo "Compiling..."
	ant compile -Dplatform=$PLATFORM

	echo "Translating in Dalvik bytecode..."
	$DX --dex --output=classes.dex obj

	echo "Making APK..."
	$AAPT package -f -m -F bin/app.unaligned.apk -M AndroidManifest.xml -S res -I $PLATFORM
	$AAPT add bin/app.unaligned.apk classes.dex

	echo "Aligning and signing APK..."
	$APKSIGNER sign --ks debug.keystore --ks-pass "pass:123456" bin/app.unaligned.apk
	$ZIPALIGN -f 4 bin/app.unaligned.apk bin/app.apk
}

run() {
	echo "Launching..."
	$ADB install -r bin/app.apk
	$ADB shell am start -n "${PACKAGE_NAME}/.MainActivity"
	echo cls
	# $ADB logcat -s "REMODROID"
}

PACKAGE_DIR="src/$(echo ${PACKAGE_NAME} | sed 's/\./\//g')"

case $1 in
	init)
		init
		;;
	init1)
		init1
		;;
	build)
		build
		;;
	run)
		run
		;;
	build-run)
		build
		run
		;;
	*)
		echo "error: unknown argument"
		;;
esac
