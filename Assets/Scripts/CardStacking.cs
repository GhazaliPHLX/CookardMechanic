using UnityEngine;

public static class CardStacking
{
    // Pastikan semua kartu berada di Layer bernama "Card"
    public static GameObject SortTopCard(Vector3 cardCenter, Vector3 halfExtents, GameObject self)
    {
        int cardMask = LayerMask.GetMask("Card");
        if (cardMask == 0)
        {
            Debug.LogWarning("[CardStacking] Layer 'Card' tidak ditemukan. Buat layer 'Card' dan assign ke kartu.");
            // fallback: cek semua layer (lebih lambat)
            cardMask = ~0;
        }

        // Geser ke bawah sedikit (pakai Y) untuk mendeteksi kartu yang benar-benar di bawah,
        // dan perkecil tinggi box supaya tidak menangkap kartu yang berjeda jauh.
        Vector3 downOffset = new Vector3(0f, -halfExtents.y * 0.6f, 0f);
        Vector3 queryHalfExtents = new Vector3(halfExtents.x * 0.9f, halfExtents.y * 0.35f, 0.1f);

        Collider[] hits = Physics.OverlapBox(cardCenter + downOffset, queryHalfExtents, Quaternion.identity, cardMask);

        GameObject topCard = null;
        int highestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            // Ambil GameObject yang punya CardComponent (naik ke parent kalau collider ada di child)
            var cardComp = hit.GetComponentInParent<CardComponent>();
            if (cardComp == null) continue;

            GameObject cardObj = cardComp.gameObject;

            // Abaikan self
            if (cardObj == self) continue;

            // Ambil SpriteRenderer dari root cardObj
            var sr = cardObj.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > highestOrder)
            {
                highestOrder = sr.sortingOrder;
                topCard = cardObj;
            }
        }

        // debug (aktifkan jika mau)
        // if (topCard != null) Debug.Log("[CardStacking] Found top: " + topCard.name + " order=" + topCard.GetComponent<SpriteRenderer>().sortingOrder);

        return topCard;
    }
}
