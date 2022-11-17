use std::{str::FromStr, sync::Arc, path::Path, fs};

use reqwest::{ Client, ClientBuilder, cookie::Jar, Url };

pub async fn get_input(year: i32, day: i32, bypass_cache: bool) -> Option<String> {
    let name = format!("cache_input_{}_{}.txt", year, day);
    let dir = Path::new(".cache");
    
    if !Path::exists(dir) {
        std::fs::create_dir(dir).expect("Failed to create input cache dire");
    }
    
    let path = dir.join(name);
    let path = path.as_path();
    if Path::exists(path) && !bypass_cache
    {
        if let Ok(content) = fs::read_to_string(path) {
            println!("Using cached input");
            return Some(content);
        }
    }

    if let Ok(api_key) = std::env::var("AOC_SESSION_KEY") 
    {
        let cookies = Arc::from(get_cookie(&api_key));
        if let Ok(client) = get_client(cookies) {
            let url = format!("https://adventofcode.com/{}/day/{}/input", year, day);
            if let Ok(response) = client.get(url
                ).send()
                .await 
            {
                let result = response.text().await.ok();
                if let Some(content) = &result {
                    fs::write(path, content).expect("Failed to save input in cache");
                } 

                return result;
            }
        }
    }
    
    None
}

fn get_client(cookies: Arc<Jar>) -> Result<Client, reqwest::Error> {
    let builder = ClientBuilder::new();
    let builder = builder.cookie_provider(cookies);

    builder.build()
}

fn get_cookie(api_key: &str) -> Jar {
    let jar = Jar::default();
    let cookie_string = format!("session={}", api_key);
    let url = Url::from_str("https://adventofcode.com").unwrap();
    jar.add_cookie_str(&cookie_string, &url);

    jar
}