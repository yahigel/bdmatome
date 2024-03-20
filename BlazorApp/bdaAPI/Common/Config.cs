using IdentityServer4.Models;

namespace bdaAPI.Common {
    class Config{
        public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("api1", "AnalyticalPerson")
            {
                Scopes = { "api1.read", "api1.write" } // 追加されたスコープ
            }
        };

        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientName = "Example Client Application", // クライアント名の追加
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1.read", "api1.write" }, // 更新されたスコープ
                AccessTokenLifetime = 3600, // トークンの有効期間を1時間に設定
                // 以下は公開クライアントの場合に追加する設定例
                RequireConsent = false,
                RedirectUris = { "https://localhost:7165/callback" },
                PostLogoutRedirectUris = { "https://localhost:7165/" },
                AllowedCorsOrigins = { "https://localhost:7165" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1.read", "Read Access to AnalyticalPerson API"), // 読み取り専用アクセス
            new ApiScope("api1.write", "Write Access to AnalyticalPerson API") // 書き込み専用アクセス
        };
    }
}