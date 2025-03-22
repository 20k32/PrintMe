import type React from "react"
import { useEffect, useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import { verificationService } from "../../services/emailVerificationService"
import "./assets/emailconfirmation.css"

const VerifyEmail: React.FC = () => {
    const [status, setStatus] = useState<"verifying" | "success" | "error">("verifying")
    const [error, setError] = useState<string>("")
    const location = useLocation()
    const navigate = useNavigate()

    useEffect(() => {
        const verifyEmail = async () => {
            const token = new URLSearchParams(location.search).get("token")

            if (!token) {
                setStatus("error")
                setError("No verification token provided")
                return
            }
            
            try {
                await verificationService.verifyEmail(token)
                setStatus("success")
            } catch (error: any) {
                console.error("Verification error:", error)
                setStatus("error")
                setError(error.message || "Verification failed")
            }
        }

        verifyEmail()
    }, [location])

    const handleContinue = () => {
        navigate("/main")
    }

    return (
        <div className="verify-email-container">
            <div className="verify-email-content">
                <h2 className="text-white mb-4">Email Verification</h2>

                {status === "verifying" && (
                    <div className="verify-status">
                        <div className="spinner"></div>
                        <p className="text-white">Verifying your email address...</p>
                    </div>
                )}

                {status === "success" && (
                    <div className="verify-status">
                        <i className="bi bi-check-circle-fill text-success fs-1 mb-3"></i>
                        <p className="text-white mb-4">Your email has been successfully verified!</p>
                        <button onClick={handleContinue} className="btn admin-btn admin-btn-primary">
                            Continue to Main Page
                        </button>
                    </div>
                )}

                {status === "error" && (
                    <div className="verify-status">
                        <i className="bi bi-x-circle-fill text-danger fs-1 mb-3"></i>
                        <p className="text-white mb-2">Verification failed</p>
                        {error && <p className="text-white-50 mb-4">{error}</p>}
                        <button onClick={handleContinue} className="btn admin-btn admin-btn-primary">
                            Return to Main Page
                        </button>
                    </div>
                )}
            </div>
        </div>
    )
}

export default VerifyEmail

