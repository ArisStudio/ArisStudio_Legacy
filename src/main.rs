use std::fs::File;
use std::io::{Read, stdin, Write};

fn main() {
    // let mut new_version = String::new();
    // let mut old_version = String::new();
    // println!("Enter new version:");
    // std::io::stdin().read_line(&mut new_version).unwrap();
    // println!("Enter old version:");
    // std::io::stdin().read_line(&mut old_version).unwrap();
    // new_version = new_version.trim().to_string();
    // old_version = old_version.trim().to_string();

    // change_skel_in_folder("./spr", &new_version, &old_version);
    change_skel_in_folder("./spr", "3.8.96", "3.8.75");
    // change_skel_in_folder("./spr", "3.8.75", "3.8.96");

    println!("Press any key to exit");
    stdin().read_line(&mut String::new()).unwrap();
}

fn find_version_pos(buffer: &Vec<u8>, old_version: &str) -> usize {
    let mut pos = 0;
    for i in 0..buffer.len() {
        if buffer[i] == old_version.as_bytes()[0] {
            let mut found = true;
            for j in 0..old_version.len() {
                if buffer[i + j] != old_version.as_bytes()[j] {
                    found = false;
                    break;
                }
            }

            if found {
                pos = i;
                break;
            }
        }
    }

    // println!("{}", pos);

    pos
}


fn change_version(buffer: &mut Vec<u8>, new_version: &str, old_version: &str, file_name: &str) -> Result<Vec<u8>, std::io::Error>
{
    let pos = find_version_pos(buffer, old_version);
    if pos == 0 {
        return Err(std::io::Error::new(std::io::ErrorKind::Other, "Version not found"));
    }
    for i in 0..new_version.len() {
        buffer[pos + i] = new_version.as_bytes()[i];
    }
    println!("{}: {} <- {}", file_name, new_version, old_version);

    Ok(buffer.to_vec())
}


fn change_skel_in_folder(folder: &str, new_version: &str, old_version: &str) {
    if !std::path::Path::new(folder).exists() {
        println!("Folder {} not found", folder);
        return;
    }

    println!("Folder: {}", folder);

    for entry in std::fs::read_dir(folder).unwrap() {
        let entry = entry.unwrap();
        let path = entry.path();
        if path.is_dir() {
            change_skel_in_folder(path.to_str().unwrap(), new_version, old_version);
        } else {
            let mut file = File::open(&path).unwrap();
            let mut buffer = Vec::new();
            file.read_to_end(&mut buffer).unwrap();

            let buffer = match change_version(&mut buffer, new_version, old_version, path.file_name().unwrap().to_str().unwrap()) {
                Ok(buffer) => buffer,
                Err(_) => continue,
            };
            let mut file = File::create(&path).unwrap();
            file.write_all(&buffer).unwrap();
        }
    }

    println!("Done!");
}