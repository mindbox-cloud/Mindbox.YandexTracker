open System
open System.IO

// Define the directory you want to search for *.cs files
let directory = @"/Users/gizdatullin/RiderProjects/Mindbox.YandexTracker"

// Define the copyright notice
let copyrightNotice = """// Copyright 2024 Mindbox Ltd
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

"""

let addCopyrightToFile (filePath: string) =
    let content = File.ReadAllText(filePath)
    if not (content.StartsWith(copyrightNotice)) then
        let newContent = copyrightNotice + content
        File.WriteAllText(filePath, newContent)
        printfn "Updated: %s" filePath

let rec processDirectory (directory: string) =
    for file in Directory.GetFiles(directory, "*.cs") do
        addCopyrightToFile file
    for subdir in Directory.GetDirectories(directory) do
        processDirectory subdir

// Start processing the directory
processDirectory directory
