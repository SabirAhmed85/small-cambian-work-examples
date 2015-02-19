param([String]$file)
$newFile = "temp_" + $file
select-string $file -pattern "^GO|^print|^$" -NotMatch | Select -ExpandProperty Line | Out-File $newFile -Encoding UTF8
remove-item $file
rename-item $newFile $file
