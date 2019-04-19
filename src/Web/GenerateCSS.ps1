
$lessFilePath = "./Code/less";
$subDir = $(Get-ChildItem "$lessFilePath");

$cssFilePath = "./Content/themes";
$cssDir = $(Get-Item "$cssFilePath");

foreach($sub in $subDir) {
    $files = $(Get-Childitem $sub.FullName -Filter *.less);

    foreach($file in $files) {
		$lessCommand = "lessc $($file.FullName) > $($sub.FullName)/$($file.BaseName).css";
		iex "& $lessCommand";
		Move-Item -Path "$($sub.FullName)/$($file.BaseName).css" -Destination "$($cssDir.FullName)/$($sub.Name)/$($file.BaseName).css" -Force
    }
}

