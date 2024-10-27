export default async function postAsync(url, body){
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    });
    return response;
}

export async function getAsync(url){
    const response = await fetch(url);
    return response;
}

export async function deleteAsync(url){
    const response = await fetch(url, {
        method: "DELETE"
    });
    return response;
}
