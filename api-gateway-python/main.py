from fastapi import FastAPI, HTTPException
import httpx
import os
from tenacity import retry, stop_after_attempt, wait_fixed

app = FastAPI(title="Gateway de Logística")
ROTA_SERVICE_URL = os.getenv("ROTA_SERVICE_URL", "http://localhost:8080")

@retry(stop=stop_after_attempt(3), wait=wait_fixed(2), reraise=True)
async def fetch_rota(origem: str, destino: str):
    timeout = httpx.Timeout(2.0)
    async with httpx.AsyncClient(timeout=timeout) as client:
        response = await client.get(f"{ROTA_SERVICE_URL}/api/rotas/calcular?origem={origem}&destino={destino}")
        response.raise_for_status()
        return response.json()

@app.get("/api/gateway/rota")
async def get_rota(origem: str, destino: str):
    try:
        return await fetch_rota(origem, destino)
    except httpx.RequestError as exc:
        raise HTTPException(status_code=503, detail=f"Serviço indisponível. Erro: {str(exc)}")
    except httpx.HTTPStatusError as exc:
        raise HTTPException(status_code=exc.response.status_code, detail=exc.response.json())
