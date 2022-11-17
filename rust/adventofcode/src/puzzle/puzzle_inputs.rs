pub async fn get_input(year: i32, day: i32, bypass_cache: bool) -> Option<String> {
    if let Ok(api_key) = std::env::var("AOC_SESSION_KEY") 
    {
        let url = format!("https://adventofcode.com/{}/day/{}/input", year, day);
        let input = reqwest::get(url)
            .await.ok()?
            .text()
            .await;

        return match input {
            Ok(t) => Some(t),
            Err(e) => None
        }
    }
    
    None
}